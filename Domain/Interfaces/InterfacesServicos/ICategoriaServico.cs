using Entities.Entidades;

namespace Domain.Interfaces.InterfacesServicos
{
    public interface ICategoriaServico
    {
        Task AdicionarCartegoria(Categoria categoria);
        Task AlterarCartegoria(Categoria categoria);
    }
}
