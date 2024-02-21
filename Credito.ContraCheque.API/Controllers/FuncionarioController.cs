using Credito.ContraCheque.API.Domain.Response;
using Credito.ContraCheque.API.Services.Commands;
using Credito.ContraCheque.API.Services.Queries;
using Credito.ContraCheque.API.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Credito.ContraCheque.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class FuncionarioController
   : BaseController
    {

        readonly IMediator _mediator;
        public FuncionarioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Busca as informações do Funcionario por Id ou de Todos os Funcionarios
        /// </summary>
        /// <param name="id">Id Funcionario</param>
        /// <returns>Se o id for fornecido, retorna o funcionario, caso não seja, retorna todos os funcionarios</returns>        
        [HttpGet(Name = nameof(ObterDadosFuncionarioAsync))]
        [ProducesResponseType(typeof(IEnumerable<DadosFuncionarioResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ObterDadosFuncionarioAsync([FromQuery] Guid? id)
            => GetResponse(await _mediator.Send(new ObterFuncionarioQuery { IdFuncionario = id }, default));

        /// <summary>
        /// Insere um novo Funcionario
        /// </summary>
        /// <param name="comando">Dados do Funcionario novo | Atencão Setor lista de setores disponiveis 
        /// { 6 Rh, 5 Adm, 4 ContasAPagar, 3 Financeiro, 2 Gerencia, 1 Ti, 0 Comercial}</param>
        /// <returns>Os dados completos do novo funcionario</returns>        
        [HttpPost(Name = nameof(InserirFuncionarioAsync))]
        [ProducesResponseType(typeof(FuncionarioCriadoResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> InserirFuncionarioAsync([FromBody] InserirFuncionarioCommand comando)
           => GetResponse(await _mediator.Send(comando, default));
    }
}
