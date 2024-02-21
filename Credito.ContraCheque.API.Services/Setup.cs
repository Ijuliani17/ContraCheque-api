using AutoMapper;
using Credito.ContraCheque.API.Services.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Credito.ContraCheque.Api.Services
{
    [ExcludeFromCodeCoverage]
    public static class Setup
    {
        public static void ConfigurarServicos(this IServiceCollection servicos, IConfiguration configuration = default)
        {
            servicos
                .AddSingleton(new MapperConfiguration(mc => { mc.AddProfile(new FuncionarioProfile()); }).CreateMapper())
                .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        }
    }
}
