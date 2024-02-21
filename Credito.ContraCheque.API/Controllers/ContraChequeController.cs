using Credito.ContraCheque.API.Domain.Response;
using Credito.ContraCheque.API.Services.Queries;
using Credito.ContraCheque.API.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Credito.ContraCheque.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ContraChequeController
       : BaseController
    {

        readonly IMediator _mediator;
        public ContraChequeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retorna Extrado do Funcionario
        /// </summary>
        /// <param name="id">Id Funcionario</param>
        /// <returns></returns>        
        [HttpGet("{id}", Name = nameof(ObterPorIdFuncionarioAsync))]
        [ProducesResponseType(typeof(ExtratoFuncionarioResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ObterPorIdFuncionarioAsync([FromRoute] Guid id)
            => GetResponse(await _mediator.Send(new ObterExtratoFuncionarioQuery { IdFuncionario = id }, default));
    }
}
