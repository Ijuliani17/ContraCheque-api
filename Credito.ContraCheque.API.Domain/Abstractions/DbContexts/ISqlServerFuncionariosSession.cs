using System.Data;

namespace Credito.ContraCheque.API.Domain.Abstractions.DbContexts
{
    public interface ISqlServerFuncionariosSession
        : IDisposable
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }
    }
}
