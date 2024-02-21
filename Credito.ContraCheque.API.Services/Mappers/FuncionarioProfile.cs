using AutoMapper;
using Credito.ContraCheque.API.Domain.Entities;
using Credito.ContraCheque.API.Domain.Enums;
using Credito.ContraCheque.API.Domain.Extensions;
using Credito.ContraCheque.API.Domain.Response;
using Credito.ContraCheque.API.Services.Commands;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Credito.ContraCheque.API.Services.Mappers
{
    [ExcludeFromCodeCoverage]
    public class FuncionarioProfile
        : Profile
    {
        public FuncionarioProfile()
        {
            CreateMap<Funcionario, DadosFuncionarioResponse>()
                .ForMember(dest => dest.NomeCompleto, cfg => cfg.MapFrom(src => $"{src.Nome} {src.Sobrenome}"))
                .ForMember(dest => dest.Documento, cfg => cfg.MapFrom(src => src.Documento.ToString(@"000\.000\.000\-00")))
                .ForMember(dest => dest.Setor, cfg => cfg.MapFrom(src => src.Setor.GetDescription()))
                .ForMember(dest => dest.salarioBruto, cfg => cfg.MapFrom(src => src.salarioBruto.ToString("C", new CultureInfo("pt-BR"))))
                .ForMember(dest => dest.SalarioLiquido, cfg => cfg.MapFrom(src => src.CalculoSalarioLiquido().ToString("C", new CultureInfo("pt-BR"))))
                .ForMember(dest => dest.DataAdmissao, cfg => cfg.MapFrom(src => $"{src.DataAdmissao:dd-MM-yyyy}"))
                .ForMember(dest => dest.PlanoSaude, cfg => cfg.MapFrom(src => $"{(src.TemDescontoPlanoSaude.Equals(TipoAdesao.Sim) ? "Aderido" : "Não Aderido")}"))
                .ForMember(dest => dest.PlanoDental, cfg => cfg.MapFrom(src => $"{(src.TemDescontoPlanoDental.Equals(TipoAdesao.Sim) ? "Aderido" : "Não Aderido")}"))
                .ForMember(dest => dest.ValeTransporte, cfg => cfg.MapFrom(src => $"{(src.TemDescontoVT.Equals(TipoAdesao.Sim) ? "Aderido" : "Não Aderido")}"));

            CreateMap<Funcionario, FuncionarioCriadoResponse>()
                .ForMember(dest => dest.Documento, cfg => cfg.MapFrom(src => src.Documento.ToString(@"000\.000\.000\-00")))
                .ForMember(dest => dest.Id, cfg => cfg.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Setor, cfg => cfg.MapFrom(src => src.Setor.GetDescription()))
                .ForMember(dest => dest.salarioBruto, cfg => cfg.MapFrom(src => src.salarioBruto.ToString("C", new CultureInfo("pt-BR"))))
                .ForMember(dest => dest.SalarioLiquido, cfg => cfg.MapFrom(src => src.CalculoSalarioLiquido().ToString("C", new CultureInfo("pt-BR"))))
                .ForMember(dest => dest.DataAdmissao, cfg => cfg.MapFrom(src => $"{src.DataAdmissao:dd-MM-yyyy}"))
                .ForMember(dest => dest.TemDescontoPlanoSaude, cfg => cfg.MapFrom(src => $"{(src.TemDescontoPlanoSaude.Equals(TipoAdesao.Sim) ? "Sim" : "Não")}"))
                .ForMember(dest => dest.TemDescontoPlanoDental, cfg => cfg.MapFrom(src => $"{(src.TemDescontoPlanoDental.Equals(TipoAdesao.Sim) ? "Sim" : "Não")}"))
                .ForMember(dest => dest.TemDescontoVT, cfg => cfg.MapFrom(src => $"{(src.TemDescontoVT.Equals(TipoAdesao.Sim) ? "Sim" : "Não")}"));

            CreateMap<InserirFuncionarioCommand, Funcionario>()
                .ForMember(dest => dest.Documento, cfg => cfg.MapFrom(src => Convert.ToInt64(src.Documento)));

            CreateMap<Funcionario, ExtratoFuncionarioResponse>()
               .ForMember(dest => dest.MesReferencia, cfg => cfg.MapFrom(src => DateTime.Now.ToString("MM-yyyy")))
               .ForMember(dest => dest.salarioBruto, cfg => cfg.MapFrom(src => src.salarioBruto.ToString("C", new CultureInfo("pt-BR"))))
               .ForMember(dest => dest.SalarioLiquido, cfg => cfg.MapFrom(src => src.CalculoSalarioLiquido().ToString("C", new CultureInfo("pt-BR"))))
               .ForMember(dest => dest.TotalDescontos, cfg => cfg.MapFrom(src => $"-{src.DescontoBeneficios.ToString("C", new CultureInfo("pt-BR"))}"));
        }
    }
}
