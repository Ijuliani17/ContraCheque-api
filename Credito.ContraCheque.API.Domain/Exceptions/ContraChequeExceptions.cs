using System.Diagnostics.CodeAnalysis;

namespace Credito.ContraCheque.API.Domain.Exceptions
{

    [ExcludeFromCodeCoverage]
    [Serializable]
    public class ContraChequeExceptions : Exception
    {
        public ContraChequeExceptions() { }
        public ContraChequeExceptions(string message) : base(message) { }
        public ContraChequeExceptions(string message, Exception inner) : base(message, inner) { }
        protected ContraChequeExceptions(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
