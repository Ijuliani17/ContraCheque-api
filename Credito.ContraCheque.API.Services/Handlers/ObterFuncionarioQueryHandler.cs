using AutoMapper;
using Credito.ContraCheque.API.Domain.Abstractions.Repositories;
using Credito.ContraCheque.API.Domain.Entities;
using Credito.ContraCheque.API.Domain.Enums;
using Credito.ContraCheque.API.Domain.Response;
using Credito.ContraCheque.API.Domain.Response.Base;
using Credito.ContraCheque.API.Services.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Credito.ContraCheque.API.Services.Handlers
{
    public class ObterFuncionarioQueryHandler
        : IRequestHandler<ObterFuncionarioQuery, ResponseContract<IEnumerable<DadosFuncionarioResponse>>>
    {
        #region Configurações Basicas
        readonly ILogger<ObterFuncionarioQueryHandler> _logger;
        readonly IMapper _mapper;
        #endregion

        #region Repositorios 
        readonly IFuncionariosRepository _funcionariosRepository;
        #endregion 

        public ObterFuncionarioQueryHandler(ILogger<ObterFuncionarioQueryHandler> logger, IFuncionariosRepository funcionariosRepository, IMapper mapper)
        {
            _logger = logger;
            _funcionariosRepository = funcionariosRepository;
            _mapper = mapper;
        }
        public async Task<ResponseContract<IEnumerable<DadosFuncionarioResponse>>> Handle(ObterFuncionarioQuery request, CancellationToken cancellationToken)
        {
            var saoTodosOsFuncionarios = request.IdFuncionario is null;

            _logger.LogInformation($"Buscando o(s) cliente(s) - [{(saoTodosOsFuncionarios ? "Todos os funcionários" : $"Cliente:{request.IdFuncionario}")}]");

            try
            {
                if (saoTodosOsFuncionarios)
                {
                    var resultadoConsulta = await _funcionariosRepository.ObterFuncionariosAsync();

                    return
                        GerarResposta(resultadoConsulta, MotivoErro.NoContent, "Não há funcionarios cadastrados.");
                }

                var resultadoConsultaUnitario = await _funcionariosRepository.ObterFuncionarioPorIdAsync(request.IdFuncionario ?? throw new ArgumentException("Id nulo", nameof(request)));

                return
                    GerarResposta(resultadoConsultaUnitario, MotivoErro.NotFound, "Não foi possível encontrar o funcionário informado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro na consulta de funcionario(s)");
                return ResponseContract<IEnumerable<DadosFuncionarioResponse>>
                    .ComDescricaoErro(MotivoErro.InternalServerError, "Ocorreu um erro ao buscar.");
            }

        }

        ResponseContract<IEnumerable<DadosFuncionarioResponse>> GerarResposta(IEnumerable<Funcionario> resultado, MotivoErro motivo, string descricaoErro)
        {
            if (resultado.Any() is false)
                return ResponseContract<IEnumerable<DadosFuncionarioResponse>>
                    .ComDescricaoErro(motivo, descricaoErro);

            var resultadoResponse = _mapper.Map<IEnumerable<DadosFuncionarioResponse>>(resultado);

            return ResponseContract<IEnumerable<DadosFuncionarioResponse>>
                .ComSucesso(resultadoResponse);
        }
    }
}
