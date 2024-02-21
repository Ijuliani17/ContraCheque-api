using System.Diagnostics.CodeAnalysis;

namespace Credito.ContraCheque.API.Domain.Response
{
    [ExcludeFromCodeCoverage]
    public struct FuncionarioCriadoResponse
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Documento { get; set; }
        public string Setor { get; set; }
        public string salarioBruto { get; set; }
        public string SalarioLiquido { get; set; }
        public string DataAdmissao { get; set; }
        public string TemDescontoPlanoSaude { get; set; }
        public string TemDescontoPlanoDental { get; set; }
        public string TemDescontoVT { get; set; }
    }
}
