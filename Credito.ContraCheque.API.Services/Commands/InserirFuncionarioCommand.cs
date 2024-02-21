using Credito.ContraCheque.API.Domain.Enums;
using Credito.ContraCheque.API.Domain.Response;
using Credito.ContraCheque.API.Domain.Response.Base;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Credito.ContraCheque.API.Services.Commands
{
    [ExcludeFromCodeCoverage]
    public struct InserirFuncionarioCommand
        : IRequest<ResponseContract<FuncionarioCriadoResponse>>
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Sobrenome { get; set; }
        [Required]
        [MaxLength(11)]
        public string Documento { get; set; }
        [Required]
        public int Setor { get; set; }
        [Required]
        public decimal salarioBruto { get; set; }
        [Required]
        public DateTime DataAdmissao { get; set; }
        [Required]
        public TipoAdesao TemDescontoPlanoSaude { get; set; }
        [Required]
        public TipoAdesao TemDescontoPlanoDental { get; set; }
        [Required]
        public TipoAdesao TemDescontoVT { get; set; }
    }
}
