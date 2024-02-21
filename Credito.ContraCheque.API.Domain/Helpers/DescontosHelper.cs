using Credito.ContraCheque.API.Domain.Extensions;

namespace Credito.ContraCheque.API.Domain.Helpers
{
    public abstract class DescontosHelper
    {
        static Dictionary<string, decimal> BaseDescontos => new()
        {
            {"Inss:1", 1045m},
            {"Inss:2", 2089.60m},
            {"Inss:3", 3134.40m},
            {"Inss:4", 6101.06m},

            {"IR:1", 1903.90m},
            {"IR:2", 2826.65m},
            {"IR:3", 3751.05m},
            {"IR:4", 4664.68m}
        };

        static Dictionary<string, decimal> TetoDescontoIR => new()
        {
            {"IR:1", (decimal)142.8},
            {"IR:2", (decimal)354.8},
            {"IR:3", (decimal)636.13},
            {"IR:4", (decimal)869.36}
        };

        public static decimal CalcularInssSalario(decimal salarioBruto) 
            => salarioBruto switch
            {
                _ when salarioBruto <= BaseDescontos.GetValueOrDefault("Inss:1")
                    => salarioBruto.CalcularDesconto(),

                _ when salarioBruto >= BaseDescontos.GetValueOrDefault("Inss:1") && salarioBruto <= BaseDescontos.GetValueOrDefault("Inss:2")
                    => salarioBruto.CalcularDesconto(percentual: 0.09),

                _ when salarioBruto >= BaseDescontos.GetValueOrDefault("Inss:2") && salarioBruto <= BaseDescontos.GetValueOrDefault("Inss:3")
                    => salarioBruto.CalcularDesconto(percentual: 0.12),

                _ when salarioBruto >= BaseDescontos.GetValueOrDefault("Inss:3") && salarioBruto <= BaseDescontos.GetValueOrDefault("Inss:4")
                    => salarioBruto.CalcularDesconto(percentual: 0.14),

                _ when salarioBruto > BaseDescontos.GetValueOrDefault("Inss:4")
                   => salarioBruto.CalcularDesconto(percentual: 0.14),

                _ => throw new NotImplementedException("Alíquota não cadastrada.")
            };

        public static decimal CalcularIRRetidoSalario(decimal salarioBruto)
            => salarioBruto switch
            {
                _ when salarioBruto < 1903.98m => default,

                _ when salarioBruto >= BaseDescontos.GetValueOrDefault("IR:1") && salarioBruto <= BaseDescontos.GetValueOrDefault("IR:2")
                    => salarioBruto.CalcularDesconto(percentual: 0.075).CalcularTetoIR(teto: 142.8m),

                _ when salarioBruto >= BaseDescontos.GetValueOrDefault("IR:2") && salarioBruto <= BaseDescontos.GetValueOrDefault("IR:3")
                    => salarioBruto.CalcularDesconto(percentual: 0.15).CalcularTetoIR(teto: 354.8m),

                _ when salarioBruto >= BaseDescontos.GetValueOrDefault("IR:3") && salarioBruto <= BaseDescontos.GetValueOrDefault("IR:4")
                    => salarioBruto.CalcularDesconto(percentual: 0.225).CalcularTetoIR(teto: 636.13m),

                _ when salarioBruto > BaseDescontos.GetValueOrDefault("IR:4")
                   => salarioBruto.CalcularDesconto(percentual: 0.275).CalcularTetoIR(teto: 869.36m),

                _ => throw new NotImplementedException("Alíquota não cadastrada.")
            };

    }
}
