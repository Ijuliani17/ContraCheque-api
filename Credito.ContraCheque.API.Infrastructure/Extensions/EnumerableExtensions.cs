namespace Credito.ContraCheque.API.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TDado> RetornaListaNaoNula<TDado>(this IEnumerable<TDado> dados)
            => dados ?? Enumerable.Empty<TDado>();
    }
}
