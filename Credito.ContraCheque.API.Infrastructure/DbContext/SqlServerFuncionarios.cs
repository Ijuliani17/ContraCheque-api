using Credito.ContraCheque.API.Domain.Abstractions.DbContexts;
using Credito.ContraCheque.API.Domain.Settings;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Text;

namespace Credito.ContraCheque.API.Infrastructure.DbContext
{
    internal sealed class SqlServerFuncionariosSession
        : ISqlServerFuncionariosSession
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        readonly IDbSettings _dbSettings;

        static string DbFile { get; set; } = $@"{AppDomain.CurrentDomain.BaseDirectory}\DB\RecursosHumanos.db";

        public SqlServerFuncionariosSession(IDbSettings settings)
        {
            _dbSettings = settings;
            CriarTabelaBase();

            Connection = new SqliteConnection(settings.ConnectionSqlServerFuncionarios);
            Connection.Open();
        }

        bool CriarTabelaBase()
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(DbFile)))
                    Directory.CreateDirectory(Path.GetDirectoryName(DbFile));

                if (File.Exists(DbFile))
                    return true;

                StreamWriter file = new StreamWriter(DbFile, true, Encoding.Default);
                file.Dispose();

                using (var connection = new SqliteConnection(_dbSettings.ConnectionSqlServerFuncionarios))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                                CREATE TABLE Funcionarios (
                                    Id TEXT PRIMARY KEY,
                                    Nome TEXT NOT NULL,
                                    Sobrenome TEXT NOT NULL,
                                    Documento INTEGER NOT NULL,
                                    Setor INTEGER NOT NULL,
                                    salarioBruto REAL NOT NULL,
                                    DataAdmissao TEXT NOT NULL,
                                    TemDescontoPlanoSaude INTEGER NOT NULL,
                                    TemDescontoPlanoDental INTEGER NOT NULL,
                                    TemDescontoValeTransporte INTEGER NOT NULL
                                );
                            ";
                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose() => Connection?.Dispose();
    }
}