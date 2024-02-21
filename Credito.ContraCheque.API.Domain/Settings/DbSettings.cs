using System.Diagnostics.CodeAnalysis;

namespace Credito.ContraCheque.API.Domain.Settings
{
    [ExcludeFromCodeCoverage]
    public class DbSettings
        : IDbSettings
    {
        public string ConnectionSqlServerFuncionarios { get; set; }
    }

    public interface IDbSettings
    {
        public string ConnectionSqlServerFuncionarios { get; set; }
    }
}
