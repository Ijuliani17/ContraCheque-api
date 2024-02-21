using Credito.ContraCheque.API.Domain.Response;
using Credito.ContraCheque.API.Domain.Response.Base;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Credito.ContraCheque.API.Services.Queries
{
    [ExcludeFromCodeCoverage]
    public struct ObterFuncionarioQuery
        : IRequest<ResponseContract<IEnumerable<DadosFuncionarioResponse>>>
    {
        public Guid? IdFuncionario { get; set; }
    }
}
