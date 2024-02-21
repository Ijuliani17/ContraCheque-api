using AutoMapper;
using Credito.ContraCheque.API.Domain.Abstractions.Repositories;
using Credito.ContraCheque.API.Infrastructure.Repositories;
using Credito.ContraCheque.API.Services.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Credito.ContraCheque.API.Tests
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseTest
    {

        private IServiceProvider _serviceProvider;

        IServiceCollection GetServiceCollection()
        {
            var assembly = typeof(Credito.ContraCheque.Api.Services.Setup).Assembly;

            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddSingleton<ILoggerFactory, LoggerFactory>()
                .AddSingleton<ILogger>(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("Test"))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly))
                .AddScoped<IFuncionariosRepository, FuncionariosRepository>()
                .AddSingleton(new MapperConfiguration(mc => { mc.AddProfile(new FuncionarioProfile()); }).CreateMapper());

            return serviceCollection;
        }

        IServiceProvider GetServiceProvider()
            => GetServiceCollection()
                .BuildServiceProvider();
        protected IServiceProvider GetDefaultServiceProvider()
            => _serviceProvider ??= GetServiceProvider();
        protected TService GetService<TService>() where TService : class
            => GetDefaultServiceProvider().GetRequiredService<TService>();

        protected Type ObterClasse(string name)
            => typeof(Credito.ContraCheque.Api.Services.Setup).Assembly.GetTypes()
                .First(classe => classe.Name.Equals(name));

    }
}