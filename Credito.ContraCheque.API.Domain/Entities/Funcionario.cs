using Credito.ContraCheque.API.Domain.Enums;
using Credito.ContraCheque.API.Domain.Extensions;
using Credito.ContraCheque.API.Domain.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Credito.ContraCheque.API.Domain.Entities
{
    public class Funcionario
    {
        public string Id { get; set; }
        [JsonIgnore]
        public Guid ExternalId { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        [MaxLength(11)]
        public long Documento { get; set; }
        public TipoSetor Setor { get; set; }
        public decimal salarioBruto { get; set; }
        public DateTime DataAdmissao { get; set; }
        public TipoAdesao TemDescontoPlanoSaude { get; set; }
        public TipoAdesao TemDescontoPlanoDental { get; set; }
        public TipoAdesao TemDescontoVT { get; set; }
        [JsonIgnore]
        public decimal DescontoBeneficios { get; set; }
       


        public decimal CalculoSalarioLiquido()
        {
            try
            {
                if (salarioBruto.Equals(default))
                    return default;

                DescontoBeneficios += CalcularInssSalario()
                                        .CalcularIRRetidoSalario()
                                        .DescontarPlanoSaude()
                                        .DescontarPlanoDentario()
                                        .DescontarValeTransporte()
                                        .DescontarFGTS();

                if (DescontoBeneficios > salarioBruto)
                    return default;

                return salarioBruto - DescontoBeneficios;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.InnerException}");
                return default;
            }
        }

        #region Regras de Desconto
        Funcionario CalcularInssSalario()
        {
            DescontoBeneficios += DescontosHelper.CalcularInssSalario(salarioBruto);
            return this;
        }
        Funcionario CalcularIRRetidoSalario()
        {
            DescontoBeneficios += DescontosHelper.CalcularIRRetidoSalario(salarioBruto); ;
            return this;
        }
        Funcionario DescontarPlanoSaude()
        {
            if (TemDescontoPlanoSaude.Equals(TipoAdesao.Sim))
                DescontoBeneficios += 10;

            return this;
        }
        Funcionario DescontarPlanoDentario()
        {
            if (TemDescontoPlanoDental.Equals(TipoAdesao.Sim))
                DescontoBeneficios += 5;

            return this;
        }
        Funcionario DescontarValeTransporte()
        {
            if (TemDescontoVT.Equals(TipoAdesao.Sim) && salarioBruto >= 1500)
                DescontoBeneficios += salarioBruto.CalcularDesconto(0.06);

            return this;
        }
        decimal DescontarFGTS()
            => (DescontoBeneficios += salarioBruto.CalcularDesconto(0.08));

        #endregion
    }
}
