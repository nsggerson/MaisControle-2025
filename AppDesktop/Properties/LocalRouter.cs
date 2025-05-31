using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;

namespace Helpers
{
    public static class LocalRouter
    {
        private static readonly Dictionary<string, Type> _controllers = new();
        public static IServiceProvider? ServiceProvider { get; set; }

        static LocalRouter()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var controllerTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Controller"));

            foreach (var type in controllerTypes)
            {
                var name = type.Name.Replace("Controller", "", StringComparison.OrdinalIgnoreCase).ToLower();
                _controllers[name] = type;
            }
        }

        public static async Task<IResult> ExecuteAsync(string path, string? jsonBody = null)
        {
            try
            {
                var parts = path.Trim('/').Split('/');
                if (parts.Length < 2)
                    return Result.Fail("Rota inválida: esperado formato 'controller/method[/id]'", 400);

                var controllerKey = parts[0].ToLower();
                var action = parts[1].ToLower();
                var extraParams = parts.Skip(2).ToArray();

                if (!_controllers.TryGetValue(controllerKey, out var controllerType))
                    return Result.Fail($"Controller '{controllerKey}' não encontrado.", 404);

                var controllerInstance = ServiceProvider?.GetService(controllerType) ?? Activator.CreateInstance(controllerType);
                if (controllerInstance == null)
                    return Result.Fail($"Não foi possível instanciar o controller '{controllerKey}'.", 500);

                var methods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

                var matchedMethod = methods.FirstOrDefault(m =>
                {
                    var httpAttr = m.GetCustomAttributes()
                        .FirstOrDefault(attr =>
                            attr.GetType().Name.StartsWith("Get") ||
                            attr.GetType().Name.StartsWith("Post") ||
                            attr.GetType().Name.StartsWith("Put") ||
                            attr.GetType().Name.StartsWith("Delete"));

                    var methodName = httpAttr switch
                    {
                        { } a when a.GetType().GetProperty("Template")?.GetValue(a) is string template => template.ToLower(),
                        _ => m.Name.Replace("Async", "", StringComparison.OrdinalIgnoreCase).ToLower()
                    };

                    return methodName == action;
                });

                if (matchedMethod == null)
                {
                    matchedMethod = methods.FirstOrDefault(m =>
                        m.Name.Replace("Async", "", StringComparison.OrdinalIgnoreCase).ToLower() == action);
                }

                if (matchedMethod == null)
                    return Result.Fail($"Método '{action}' não encontrado em '{controllerType.Name}'.", 404);

                var parametersInfo = matchedMethod.GetParameters();
                var parameters = new object?[parametersInfo.Length];

                for (int i = 0; i < parametersInfo.Length; i++)
                {
                    var paramType = parametersInfo[i].ParameterType;

                    if (i < extraParams.Length && paramType.IsPrimitive)
                    {
                        parameters[i] = Convert.ChangeType(extraParams[i], paramType);
                    }
                    else if (paramType == typeof(string) && i < extraParams.Length)
                    {
                        parameters[i] = extraParams[i];
                    }
                    else if (jsonBody != null)
                    {
                        parameters[i] = JsonSerializer.Deserialize(jsonBody, paramType);
                    }
                }

                var result = matchedMethod.Invoke(controllerInstance, parameters);

                if (result is Task taskResult)
                {
                    await taskResult.ConfigureAwait(false);
                    var resultProperty = taskResult.GetType().GetProperty("Result");
                    var taskValue = resultProperty?.GetValue(taskResult);

                    if (taskValue is IResult taskIResult)
                        return taskIResult;

                    return Result.Ok(taskValue);
                }

                if (result is IResult iResult)
                    return iResult;

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new List<string>
            {
                $"Erro interno: {ex.Message}",
                ex.InnerException?.Message ?? "",
                ex.StackTrace ?? ""
            }, 500);
            }
        }
    }
   
    public abstract class BaseController
    {
        protected async Task<IResult> ExecutarRotaInternaAsync(string path, string? json = null)
        {
            return await LocalRouter.ExecuteAsync(path, json);
        }

        protected bool TryValidate(object model, out List<string> errors)
        {
            errors = new List<string>();

            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, context, results, true);

            if (!isValid)
            {
                errors.AddRange(results.Select(r => r.ErrorMessage ?? "Erro de validação."));
            }

            return isValid;
        }
        protected List<string> ValidateModel<T>(T model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model ?? default!);
            Validator.TryValidateObject(model ?? default!, context, results, validateAllProperties: true);
            return results.Select(r => r.ErrorMessage ?? "Campo inválido").ToList();
        }

        protected bool TryValidate<T>(T model, out List<string> errors)
        {
            errors = ValidateModel(model);
            return errors.Count == 0;
        }
    }

    // Atributos de rota
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class GetAttribute : Attribute
    {
        public string Route { get; }
        public GetAttribute(string route) => Route = route;
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PostAttribute : Attribute
    {
        public string Route { get; }
        public PostAttribute(string route) => Route = route;
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PutAttribute : Attribute
    {
        public string Route { get; }
        public PutAttribute(string route) => Route = route;
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DeleteAttribute : Attribute
    {
        public string Route { get; }
        public DeleteAttribute(string route) => Route = route;
    }

    public class Result : IResult
    {
        public bool Success { get; private set; }
        public int StatusCode { get; private set; }
        public List<string> Errors { get; private set; } = new();
        public object? Value { get; private set; }
        public string? Message { get; private set; } // ✅ Adicionado
        private Result() { }

        public static Result Ok(object? value = null, string? message = null, int statusCode = 200)
        {
            return new Result
            {
                Success = true,
                StatusCode = statusCode,
                Value = value,
                Message = message
            };
        }

        public static Result Fail(string error, int statusCode = 400)
        {
            return new Result
            {
                Success = false,
                StatusCode = statusCode,
                Errors = new List<string> { error },
                Message = null
            };
        }

        public static Result Fail(List<string> errors, int statusCode = 400)
        {
            return new Result
            {
                Success = false,
                StatusCode = statusCode,
                Errors = errors,
                Message = null
            };
        }

        public static Result Fail(string message, List<string> errors, int statusCode = 400)
        {
            return new Result
            {
                Success = false,
                StatusCode = statusCode,
                Errors = errors,
                Message = message
            };
        }

        public static Result Fail(string message, string error, int statusCode = 400)
        {
            return Fail(message, new List<string> { error }, statusCode);
        }
    }

    public interface IResult
    {
        bool Success { get; }
        int StatusCode { get; }
        List<string> Errors { get; }
        object? Value { get; }
        string? Message { get; } // ✅ Adicionado
    }
}
