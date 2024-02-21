using AutoMapper;
using Credito.ContraCheque.API.Domain.Abstractions.Repositories;
using Credito.ContraCheque.API.Domain.Entities;
using Credito.ContraCheque.API.Domain.Enums;
using Credito.ContraCheque.API.Domain.Response;
using Credito.ContraCheque.API.Domain.Response.Base;
using Credito.ContraCheque.API.Services.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Credito.ContraCheque.API.Services.Handlers
{
    public class InserirFuncionarioCommandHandler
        : IRequestHandler<InserirFuncionarioCommand, ResponseContract<FuncionarioCriadoResponse>>
    {
        #region Configurações Basicas
        readonly ILogger<InserirFuncionarioCommandHandler> _logger;
        readonly IMapper _mapper;
        #endregion

        #region Repositorios 
        readonly IFuncionariosRepository _funcionariosRepository;
        #endregion 

        public InserirFuncionarioCommandHandler(ILogger<InserirFuncionarioCommandHandler> logger, IFuncionariosRepository funcionariosRepository, IMapper mapper)
        {
            _logger = logger;
            _funcionariosRepository = funcionariosRepository;
            _mapper = mapper;
        }
        public async Task<ResponseContract<FuncionarioCriadoResponse>> Handle(InserirFuncionarioCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Inserindo novo funcionario - [Nome: {request.Nome} {request.Sobrenome}]");

            try
            {
                var novoFuncionario = await _funcionariosRepository.InserirFuncionarioAsync(_mapper.Map<Funcionario>(request));

                if (novoFuncionario is null || novoFuncionario.Id.Equals(Guid.Empty))
                    return ResponseContract<FuncionarioCriadoResponse>
                        .ComDescricaoErro(MotivoErro.BadRequest, "Não foi possível sensibilizar o novo funcionario");

                return ResponseContract<FuncionarioCriadoResponse>
                    .ComSucesso(_mapper.Map<FuncionarioCriadoResponse>(novoFuncionario));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocorreu um erro na inserção do novo Funcionario - [Nome: {request.Nome} {request.Sobrenome}]");

                return ResponseContract<FuncionarioCriadoResponse>
                    .ComDescricaoErro(MotivoErro.InternalServerError, "Ocorreu um erro ao tentar inserir o novo Funcionario.");
            }

        }
    }
}
