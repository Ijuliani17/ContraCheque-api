using AutoMapper;
using Credito.ContraCheque.Api.Domain.ValueObjects;
using Credito.ContraCheque.API.Domain.Abstractions.Repositories;
using Credito.ContraCheque.API.Domain.Entities;
using Credito.ContraCheque.API.Domain.Enums;
using Credito.ContraCheque.API.Domain.Extensions;
using Credito.ContraCheque.API.Domain.Helpers;
using Credito.ContraCheque.API.Domain.Response;
using Credito.ContraCheque.API.Domain.Response.Base;
using Credito.ContraCheque.API.Services.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Credito.ContraCheque.API.Services.Handlers
{
    public class ObterExtratoFuncionarioQueryHandler
        : IRequestHandler<ObterExtratoFuncionarioQuery, ResponseContract<ExtratoFuncionarioResponse>>
    {
        #region Configurações Basicas
        readonly ILogger<ObterExtratoFuncionarioQueryHandler> _logger;
        readonly IMapper _mapper;
        #endregion

        #region Repositorios 
        readonly IFuncionariosRepository _funcionariosRepository;
        #endregion 

        public ObterExtratoFuncionarioQueryHandler(ILogger<ObterExtratoFuncionarioQueryHandler> logger, IFuncionariosRepository funcionariosRepository, IMapper mapper)
        {
            _logger = logger;
            _funcionariosRepository = funcionariosRepository;
            _mapper = mapper;
        }

        public async Task<ResponseContract<ExtratoFuncionarioResponse>> Handle(ObterExtratoFuncionarioQuery request, CancellationToken cancellationToken)
        {

            _logger.LogInformation($"Buscando o extrato do Cliente - [IdCliente:{request.IdFuncionario}");

            try
            {
                var dadosFuncionario = (await _funcionariosRepository.ObterFuncionarioPorIdAsync(request.IdFuncionario))?.FirstOrDefault();

                if (dadosFuncionario is null || dadosFuncionario.Equals(default))
                    return ResponseContract<ExtratoFuncionarioResponse>
                        .ComDescricaoErro(MotivoErro.NotFound, "Funcionario não encontrado.");

                var dadosExtrato = _mapper.Map<ExtratoFuncionarioResponse>(dadosFuncionario);

                dadosExtrato.Lancamentos = RegistrarLancamentos(dadosFuncionario);

                return ResponseContract<ExtratoFuncionarioResponse>
                    .ComSucesso(dadosExtrato);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocorreu um erro na consulta extrato do funcionario - [Cliente:{request.IdFuncionario}");
                return ResponseContract<ExtratoFuncionarioResponse>
                    .ComDescricaoErro(MotivoErro.InternalServerError, "Ocorreu um erro ao gerar Extrato.");
            }
        }
        IEnumerable<Lancamento> RegistrarLancamentos(Funcionario funcionario)
        {
            List<Lancamento> lancamentosPadrao = new()
            {
                new Lancamento
                    {
                        Valor = funcionario.salarioBruto,
                        Descricao = "Remuneração",
                        Tipo = TipoLancamento.Remuneracao
                    },
                new Lancamento
                    {
                        Valor = RetornaValorDesconto("IRRF", funcionario.salarioBruto),
                        Descricao = "IRRF",
                        Tipo = TipoLancamento.Desconto
                    },
                new Lancamento
                    {
                        Valor = RetornaValorDesconto("INSS", funcionario.salarioBruto),
                        Descricao = "INSS",
                        Tipo = TipoLancamento.Desconto
                    },
                new Lancamento
                    {
                        Valor = RetornaValorDesconto("FGTS", funcionario.salarioBruto),
                        Descricao = "FGTS",
                        Tipo = TipoLancamento.Desconto
                    }
            };

            var lancamentosDesconto = funcionario
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
                .Where(propriedade => propriedade.Name.Contains("TemD", StringComparison.InvariantCultureIgnoreCase) 
                    && ((TipoAdesao)propriedade.GetValue(funcionario)).Equals(TipoAdesao.Sim))
                .Select(propriedade =>
                {
                    var valor = RetornaValorDesconto(propriedade.Name, funcionario.salarioBruto);
                    var descricao = propriedade.Name.Replace("TemDesconto", string.Empty);

                    return new Lancamento
                    {
                        Valor = valor,
                        Descricao = descricao,
                        Tipo = TipoLancamento.Desconto
                    };
                }).ToList();

            lancamentosDesconto.AddRange(lancamentosPadrao);

            return lancamentosDesconto;
        }
        decimal RetornaValorDesconto(string desconto, decimal salarioBruto)
            => desconto switch
            {
                _ when desconto.Contains("Saude", StringComparison.InvariantCultureIgnoreCase) => 10m,

                _ when desconto.Contains("Dental", StringComparison.InvariantCultureIgnoreCase) => 5m,

                _ when desconto.Contains("VT", StringComparison.InvariantCultureIgnoreCase) && salarioBruto >= 1500m
                    => salarioBruto.CalcularDesconto(percentual: 0.06),

                _ when desconto.Equals("IRRF", StringComparison.InvariantCultureIgnoreCase)
                    => DescontosHelper.CalcularIRRetidoSalario(salarioBruto),

                _ when desconto.Equals("INSS", StringComparison.InvariantCultureIgnoreCase)
                    => DescontosHelper.CalcularInssSalario(salarioBruto),

                _ => salarioBruto.CalcularDesconto(0.08)
            };
    }
}
