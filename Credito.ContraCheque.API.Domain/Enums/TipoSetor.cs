using System.ComponentModel;

namespace Credito.ContraCheque.API.Domain.Enums
{
    public enum TipoSetor
    {
        [Description("Comercial")]
        Comercial,
        [Description("Tecnologia da Informação")]
        Ti,
        [Description("Gerência")]
        Gerencia,
        [Description("Financeiro")]
        Financeiro,
        [Description("Contas a Pagar")]
        ContasAPagar,
        [Description("Administrativo")]
        Adm,
        [Description("Recursos Humanos")]
        Rh
    }
}
