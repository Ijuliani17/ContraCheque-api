using System.Diagnostics.CodeAnalysis;

namespace Credito.ContraCheque.API.Domain.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class FinanceiroExtensions
    {
        public static decimal CalcularDesconto(this decimal valor, double percentual = 0.075)
        {
            if (valor.Equals(default))
                throw new ArgumentException("Valor vazio.", nameof(valor));

            return valor * (decimal)percentual;
        }
        public static decimal CalcularTetoIR(this decimal valorDesconto, decimal teto)
            => valorDesconto > teto
                ? teto
                : valorDesconto;
    }
}
