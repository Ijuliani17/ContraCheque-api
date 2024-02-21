using System.Diagnostics.CodeAnalysis;

namespace Credito.ContraCheque.API.Domain.Response
{
    [ExcludeFromCodeCoverage]
    public class DadosFuncionarioResponse
    {
        public string NomeCompleto { get; set; }
        public string Documento { get; set; }
        public string Setor { get; set; }
        public string salarioBruto { get; set; }
        public string SalarioLiquido { get; set; }
        public string DataAdmissao { get; set; }
        public string PlanoSaude { get; set; }
        public string PlanoDental { get; set; }
        public string ValeTransporte { get; set; }
    }
}
