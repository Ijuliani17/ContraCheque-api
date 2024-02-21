using AutoMapper;
using Credito.ContraCheque.API.Domain.Abstractions.Repositories;
using Credito.ContraCheque.API.Domain.Entities;
using Credito.ContraCheque.API.Domain.Enums;
using Credito.ContraCheque.API.Domain.Response;
using Credito.ContraCheque.API.Services.Commands;
using Credito.ContraCheque.API.Services.Handlers;
using Credito.ContraCheque.API.Services.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Credito.ContraCheque.API.Tests.Services
{
    public class ServicesTest
        : BaseTest
    {
        readonly IMediator _mediator;
        readonly Mock<IFuncionariosRepository> _repo;

        readonly Mock<ILogger<ObterFuncionarioQueryHandler>> _loggerObterFuncionarioQueryHandler;
        readonly Mock<ILogger<ObterExtratoFuncionarioQueryHandler>> _loggerObterExtratoFuncionarioQueryHandler;
        readonly Mock<ILogger<InserirFuncionarioCommandHandler>> _loggerInserirFuncionarioCommandHandler;
        readonly Mock<IMapper> _mapper;

        ObterFuncionarioQueryHandler _obterFuncionarioQueryHandler;
        ObterExtratoFuncionarioQueryHandler _obterExtratoFuncionarioQueryHandler;
        InserirFuncionarioCommandHandler _inserirFuncionarioCommandHandler;

        List<Funcionario> _funcionarios = new()
        {
            new Funcionario
             {
                 Id = Guid.NewGuid().ToString(),
                 ExternalId = Guid.NewGuid(),
                 Nome = "Teste",
                 Sobrenome = "Testando",
                 DataAdmissao = DateTime.Now,
                 Documento = 11122233344,
                 salarioBruto = 5000m,
                 Setor = Domain.Enums.TipoSetor.Ti,
                 TemDescontoPlanoSaude = Domain.Enums.TipoAdesao.Nao,
                 TemDescontoPlanoDental = Domain.Enums.TipoAdesao.Nao,
                 TemDescontoVT = Domain.Enums.TipoAdesao.Sim
             }
        };

        public ServicesTest()
        {
            _loggerObterFuncionarioQueryHandler = new Mock<ILogger<ObterFuncionarioQueryHandler>>();
            _loggerObterExtratoFuncionarioQueryHandler = new Mock<ILogger<ObterExtratoFuncionarioQueryHandler>>();
            _loggerInserirFuncionarioCommandHandler = new Mock<ILogger<InserirFuncionarioCommandHandler>>();

            _repo = new Mock<IFuncionariosRepository>();
            _mediator = GetService<IMediator>();

            _mapper = new Mock<IMapper>();

            _obterFuncionarioQueryHandler = (ObterFuncionarioQueryHandler)(ObterClasse("ObterFuncionarioQueryHandler")
                .GetConstructor(new Type[]
            {
                typeof(ILogger<ObterFuncionarioQueryHandler>),
                typeof(IFuncionariosRepository),
                typeof(IMapper)
            })).Invoke(new object[] { _loggerObterFuncionarioQueryHandler.Object, _repo.Object, _mapper.Object });

            _obterExtratoFuncionarioQueryHandler = (ObterExtratoFuncionarioQueryHandler)(ObterClasse("ObterExtratoFuncionarioQueryHandler")
               .GetConstructor(new Type[]
           {
                typeof(ILogger<ObterExtratoFuncionarioQueryHandler>),
                typeof(IFuncionariosRepository),
                typeof(IMapper)
           })).Invoke(new object[] { _loggerObterExtratoFuncionarioQueryHandler.Object, _repo.Object, _mapper.Object });

            _inserirFuncionarioCommandHandler = (InserirFuncionarioCommandHandler)(ObterClasse("InserirFuncionarioCommandHandler")
              .GetConstructor(new Type[]
          {
                typeof(ILogger<InserirFuncionarioCommandHandler>),
                typeof(IFuncionariosRepository),
                typeof(IMapper)
          })).Invoke(new object[] { _loggerInserirFuncionarioCommandHandler.Object, _repo.Object, _mapper.Object });
        }

        [Fact]
        async Task ObterFuncionarioQueryId_Sucesso()
        {
            //Arrange
            _repo.Setup(repo => repo.ObterFuncionarioPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(_funcionarios);
            //Act
            var resultado = await _obterFuncionarioQueryHandler.Handle(new ObterFuncionarioQuery { IdFuncionario = Guid.Empty }, default);

            //Assert
            Assert.False(resultado.PossuiErro);
            Assert.NotNull(resultado.Dados);
        }

        [Fact]
        async Task ObterFuncionarisoQuery_Sucesso()
        {
            //Arrange
            _repo.Setup(repo => repo.ObterFuncionariosAsync())
                .ReturnsAsync(_funcionarios);
            //Act
            var resultado = await _obterFuncionarioQueryHandler.Handle(new ObterFuncionarioQuery { IdFuncionario = null }, default);

            //Assert
            Assert.False(resultado.PossuiErro);
            Assert.NotNull(resultado.Dados);
        }

        [Fact]
        async Task ObterFuncionarisoQuery_Eror()
        {
            //Arrange
            _repo.Setup(repo => repo.ObterFuncionariosAsync())
                .ThrowsAsync(new Exception());
            //Act
            var resultado = await _obterFuncionarioQueryHandler.Handle(new ObterFuncionarioQuery { IdFuncionario = null }, default);

            //Assert
            Assert.True(resultado.PossuiErro);
            Assert.Null(resultado.Dados);
        }

        [Fact]
        async Task ObterExtratoFuncionarioQuery_Sucesso()
        {
            //Arrange
            _repo.Setup(repo => repo.ObterFuncionarioPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(_funcionarios);

            _mapper.Setup(mapper => mapper.Map<ExtratoFuncionarioResponse>(It.IsAny<Funcionario>()))
                .Returns(new ExtratoFuncionarioResponse());

            //Act
            var resultado = await _obterExtratoFuncionarioQueryHandler.Handle(new ObterExtratoFuncionarioQuery { IdFuncionario = Guid.Empty }, default);

            //Assert
            Assert.False(resultado.PossuiErro);
            Assert.NotNull(resultado.Dados);
        }

        [Fact]
        async Task ObterExtratoFuncionarioQuery_ErrorMap()
        {
            //Arrange
            _repo.Setup(repo => repo.ObterFuncionarioPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(_funcionarios);

            _mapper.Setup(mapper => mapper.Map<ExtratoFuncionarioResponse>(It.IsAny<Funcionario>()))
                .Throws(new Exception());

            //Act
            var resultado = await _obterExtratoFuncionarioQueryHandler.Handle(new ObterExtratoFuncionarioQuery { IdFuncionario = Guid.Empty }, default);

            //Assert
            Assert.True(resultado.PossuiErro);
            Assert.Null(resultado.Dados);
        }

        [Fact]
        async Task ObterExtratoFuncionarioQuery_FuncionarioNaoEncontrad()
        {
            //Arrange
            _repo.Setup(repo => repo.ObterFuncionarioPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<Funcionario> { new Funcionario() });

            //Act
            var resultado = await _obterExtratoFuncionarioQueryHandler.Handle(new ObterExtratoFuncionarioQuery { IdFuncionario = Guid.Empty }, default);

            //Assert
            Assert.True(resultado.PossuiErro);
            Assert.Null(resultado.Dados);
            Assert.Equal(MotivoErro.NotFound, resultado.MotivoErro);
        }

        [Fact]
        async Task InserirFuncionarioCommand_Sucesso()
        {
            //Arrange
            var funcionario = _funcionarios.First();

            _repo.Setup(repo => repo.InserirFuncionarioAsync(It.IsAny<Funcionario>()))
                .ReturnsAsync(funcionario);

            //Act
            var resultado = await _inserirFuncionarioCommandHandler.Handle(new InserirFuncionarioCommand
            {
                salarioBruto = funcionario.salarioBruto,
                Setor = funcionario.Setor.GetHashCode(),
                Sobrenome = funcionario.Sobrenome,
                DataAdmissao = funcionario.DataAdmissao,
                Documento = funcionario.Documento.ToString(),
                Nome = funcionario.Nome,
                TemDescontoPlanoSaude = funcionario.TemDescontoPlanoSaude,
                TemDescontoPlanoDental = funcionario.TemDescontoPlanoDental,
                TemDescontoVT = funcionario.TemDescontoVT

            }, default);

            //Assert
            Assert.False(resultado.PossuiErro);
            Assert.NotNull(resultado.Dados);
        }

        [Fact]
        async Task InserirFuncionarioCommand_Error()
        {
            //Arrange
            var funcionario = _funcionarios.First();

            _repo.Setup(repo => repo.InserirFuncionarioAsync(It.IsAny<Funcionario>()))
                .ThrowsAsync(new Exception());

            //Act
            var resultado = await _inserirFuncionarioCommandHandler.Handle(new InserirFuncionarioCommand
            {
                salarioBruto = funcionario.salarioBruto,
                Setor = funcionario.Setor.GetHashCode(),
                Sobrenome = funcionario.Sobrenome,
                DataAdmissao = funcionario.DataAdmissao,
                Documento = funcionario.Documento.ToString(),
                Nome = funcionario.Nome,
                TemDescontoPlanoSaude = funcionario.TemDescontoPlanoSaude,
                TemDescontoPlanoDental = funcionario.TemDescontoPlanoDental,
                TemDescontoVT = funcionario.TemDescontoVT

            }, default);

            //Assert
            Assert.True(resultado.PossuiErro);
        }
    }
}
