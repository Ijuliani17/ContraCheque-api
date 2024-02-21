using Credito.ContraCheque.API.Domain.Abstractions.DbContexts;
using Credito.ContraCheque.API.Domain.Abstractions.Repositories;
using Credito.ContraCheque.API.Domain.Settings;
using Credito.ContraCheque.API.Infrastructure.DbContext;
using Credito.ContraCheque.API.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Credito.ContraCheque.API.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class Setup
    {
        public static void ConfigurarInfraestrutura(this IServiceCollection servicos, Action<IDbSettings> dbSettings)
        {
            IDbSettings configureDb = new DbSettings();
            dbSettings.Invoke(configureDb);

            servicos
                .AddSingleton(configureDb)
                .AddSingleton<ISqlServerFuncionariosSession, SqlServerFuncionariosSession>()
                .AddScoped<IFuncionariosRepository, FuncionariosRepository>();
        }
    }
}
