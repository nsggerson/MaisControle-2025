using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Helpers;

/// <summary>
/// A classe EnumExtensions fornece métodos de extensão para enums, permitindo a obtenção de atributos de exibição (Display) associados aos valores do enum.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Obtém o valor do atributo Display.Name do enum.
    /// Retorna o nome do enum se o atributo não estiver presente.
    /// </summary>
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.HasDisplayAttribute() ? enumValue.GetDisplayAttribute()?.Name ?? enumValue.ToString() : enumValue.ToString();
    }

    /// <summary>
    /// Obtém o valor do atributo Display.GroupName do enum.
    /// Retorna uma string vazia se o atributo não estiver presente.
    /// </summary>
    public static string GetGroupName(this Enum enumValue)
    {
        return enumValue.HasDisplayAttribute() ? enumValue.GetDisplayAttribute()?.GroupName ?? string.Empty : string.Empty;
    }

    /// <summary>
    /// Obtém o valor do atributo Display.Order do enum.
    /// Retorna -1 se o atributo não estiver presente.
    /// </summary>
    public static int GetOrder(this Enum enumValue)
    {
        try
        {
            return enumValue.HasDisplayAttribute() ? enumValue.GetDisplayAttribute()?.Order ?? -1 : -1; // Retorna -1 se não houver valor definido
        }
        catch (Exception)
        {
            // Caso ocorra algum erro, retorna -1
            return -1;
        }
    }

    /// <summary>
    /// Obtém o valor numérico associado ao enum.
    /// </summary>
    public static int GetCode(this Enum enumValue)
    {
        return Convert.ToInt32(enumValue);
    }
    public static string GetDisplayDescription(this Enum enumValue)
    {
        return enumValue.HasDisplayAttribute() ?
               enumValue.GetDisplayAttribute()?.Description ?? string.Empty :
               string.Empty;
    }
    /// <summary>
    /// Verifica se o atributo Display está presente no enum.
    /// </summary>
    public static bool HasDisplayAttribute(this Enum enumValue)
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        var attribute = fieldInfo!.GetCustomAttributes<DisplayAttribute>(false).FirstOrDefault();
        return attribute != null;
    }

    /// <summary>
    /// Método auxiliar para obter o atributo Display do enum.
    /// Retorna o atributo Display associado ao valor do enum ou null se não houver.
    /// </summary>
    private static DisplayAttribute GetDisplayAttribute(this Enum enumValue)
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        return fieldInfo!.GetCustomAttributes<DisplayAttribute>(false).FirstOrDefault()!;
    }
}
