using Credito.ContraCheque.Api.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Credito.ContraCheque.API.Domain.Response
{
    [ExcludeFromCodeCoverage]
    public class ExtratoFuncionarioResponse
    {
        public string MesReferencia { get; set; }
        public string salarioBruto { get; set; }
        public string SalarioLiquido { get; set; }
        public string TotalDescontos { get; set; }
        public IEnumerable<Lancamento> Lancamentos { get; set; }
    }
}
