using Credito.ContraCheque.API.Domain.Entities;

namespace Credito.ContraCheque.API.Domain.Abstractions.Repositories
{
    public interface IFuncionariosRepository
    {
        Task<IEnumerable<Funcionario>> ObterFuncionarioPorIdAsync(Guid id);
        Task<IEnumerable<Funcionario>> ObterFuncionariosAsync();
        Task<Funcionario> InserirFuncionarioAsync(Funcionario funcionario);
    }
}
