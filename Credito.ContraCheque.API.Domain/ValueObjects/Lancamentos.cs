using Credito.ContraCheque.API.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Credito.ContraCheque.Api.Domain.ValueObjects
{
    [ExcludeFromCodeCoverage]
    public class Lancamento
    {
        public TipoLancamento Tipo { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
    }
}
