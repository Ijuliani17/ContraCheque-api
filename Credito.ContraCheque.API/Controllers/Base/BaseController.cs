using Credito.ContraCheque.API.Domain.Enums;
using Credito.ContraCheque.API.Domain.Response.Base;
using Microsoft.AspNetCore.Mvc;

namespace Credito.ContraCheque.API.Controllers.Base
{
    public abstract class BaseController
         : Controller
    {
        public HttpRequest Request => HttpContext?.Request;

        public IActionResult GetResponse<TResponse>(ResponseContract<TResponse> source)
           => source switch
           {
               _ when source.PossuiErro && source.MotivoErro.Equals(MotivoErro.BadRequest)
                   => BadRequest(source.DetalheErro),

               _ when source.PossuiErro && source.MotivoErro.Equals(MotivoErro.NoContent)
                   => NoContent(),

               _ when source.PossuiErro && source.MotivoErro.Equals(MotivoErro.NotFound)
                  => NotFound(source.DetalheErro),

               _ when source.PossuiErro
                   => StatusCode(500, source.DetalheErro),

               _ when source.Dados is not null
                   => Ok(source.Dados),

               _ when Request.Method!.Equals("POST")
                   => Created(),

               _ => NoContent()
           };
    }
}
