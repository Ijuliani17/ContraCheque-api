using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO.Compression;
using System.Text.Json.Serialization;
using System.Text.Json;
using Credito.ContraCheque.Api.Services;
using Credito.ContraCheque.API.Infrastructure;

namespace Credito.ContraCheque.API
{
    [ExcludeFromCodeCoverage]
    public static class Setup
    {
        const string CORS_DEFAULT_POLICY = "Default";
        public static void ConfigurarApi(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            #region Configurações de Localização

            CultureInfo ci = new("pt-BR");

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("pt-BR");
                options.SupportedCultures = new List<CultureInfo>
                {
                    new("en"),
                    ci
                };
                options.SupportedUICultures = new List<CultureInfo>
                {
                    new("en"),
                    ci
                };
            });

            #endregion

            #region Configurações Cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(CORS_DEFAULT_POLICY, config =>
                {
                    config
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });
            #endregion

            #region Configurações Controller

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            #endregion

            //Seta o Gzip como Provedor de compactação
            builder.Services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            //Irá compactar a resposta gerada pela aplicação, melhorando sua função
            builder.Services.AddResponseCompression();

            builder.Services.AddEndpointsApiExplorer();

#if DEBUG
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "API - V1",
                        Version = "v1"
                    }
                 );

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "Credito.ContraCheque.Api.xml");
                c.IncludeXmlComments(filePath);
            });
#endif

            builder.Services.ConfigurarServicos(configuration: configuration);
            builder.Services.ConfigurarInfraestrutura(dbSettings =>
            {
                dbSettings.ConnectionSqlServerFuncionarios = $@"Data Source={AppDomain.CurrentDomain.BaseDirectory}\DB\RecursosHumanos.db";
            });
        }
    }
}
