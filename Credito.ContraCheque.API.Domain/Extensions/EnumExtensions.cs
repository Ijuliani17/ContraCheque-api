using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Credito.ContraCheque.API.Domain.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumerationValue)
        {
            var tipo = enumerationValue.GetType();
            var ehEnumerador = tipo.IsEnum;

            if (ehEnumerador is false)
                throw new ArgumentException($"{nameof(enumerationValue)} precisa ser um enumerador", nameof(enumerationValue));

            var memberInfo = tipo.GetMember(enumerationValue.ToString());
            var ehMembroNaoEncontrado = memberInfo.Length <= 0;

            if (ehMembroNaoEncontrado)
                return enumerationValue.ToString();

            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attrs.Length > 0)
                return ((DescriptionAttribute)attrs[0]).Description;

            return enumerationValue.ToString();
        }
    }
}
