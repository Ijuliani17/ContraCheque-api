using Credito.ContraCheque.API.Domain.Abstractions.DbContexts;
using Credito.ContraCheque.API.Domain.Abstractions.Repositories;
using Credito.ContraCheque.API.Domain.Entities;
using Credito.ContraCheque.API.Infrastructure.Extensions;
using Dapper;

namespace Credito.ContraCheque.API.Infrastructure.Repositories
{
    public class FuncionariosRepository
        : IFuncionariosRepository
    {
        readonly ISqlServerFuncionariosSession _session;

        const int NO_ROWS_AFFECTED = 0;
        public FuncionariosRepository(
            ISqlServerFuncionariosSession session
            )
        {
            _session = session;
        }

        public async Task<IEnumerable<Funcionario>> ObterFuncionarioPorIdAsync(Guid id)
        {
            var query = string.Format(GetType().ObterConteudoArquivo("Scripts.ObterFuncionarioPorId.sql"), id.ToString().ToUpper());


            var resultado = await _session.Connection.QueryAsync<Funcionario>(query);
            return (await _session.Connection.QueryAsync<Funcionario>(query))
                .RetornaListaNaoNula();
        }

        public async Task<IEnumerable<Funcionario>> ObterFuncionariosAsync()
            => (await _session.Connection.QueryAsync<Funcionario>(GetType().ObterConteudoArquivo("Scripts.ObterFuncionarios.sql")))
                .RetornaListaNaoNula();
        public async Task<Funcionario> InserirFuncionarioAsync(Funcionario funcionario)
        {
            funcionario.ExternalId = ObterIdFuncinario();

            if ((await _session.Connection.ExecuteAsync(GetType().ObterConteudoArquivo("Scripts.InserirNovoFuncionario.sql"), funcionario)) > NO_ROWS_AFFECTED)
            {
                funcionario.Id = funcionario.ExternalId.ToString();
                return funcionario;
            }
            return default;
        }

        Guid ObterIdFuncinario()
            => Guid.NewGuid();
    }
}
