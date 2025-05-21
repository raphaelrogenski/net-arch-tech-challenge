using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetArchTechChallenge.Shared.Infrastructure.DbContexts
{
    public class DatabaseInitializer
    {
        private readonly string _connString;
        private readonly string _databaseName = "NetArch";

        public DatabaseInitializer()
        {
#if DEBUG
            _connString = $"Server=localhost;Database=master;User Id=sa;Password=SuaSenha123!;TrustServerCertificate=True;";
#else
            _connString = $"Server=svc-sqlserver;Database=master;User Id=sa;Password=SuaSenha123!;TrustServerCertificate=True;";
#endif
        }

        public async Task InitializeAsync()
        {
            await CreateDatabaseAsync();
            await CreateTablesAsync();
        }

        private async Task CreateDatabaseAsync()
        {
            using var connection = new SqlConnection(_connString);
            await connection.OpenAsync();

            var checkDbCommand = new SqlCommand(
                $@"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{_databaseName}')
                CREATE DATABASE [{_databaseName}];", connection);

            await checkDbCommand.ExecuteNonQueryAsync();

            Console.WriteLine("✅ Banco criado (ou já existia).");
        }

        private async Task CreateTablesAsync()
        {
            var databaseConnString = _connString.Replace("Database=master", $"Database={_databaseName}");

            using var connection = new SqlConnection(databaseConnString);
            await connection.OpenAsync();

            var createTableCommand = new SqlCommand(
                @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Contacts')
              BEGIN
				CREATE TABLE [dbo].[Contacts](
					[Id] [uniqueidentifier] NOT NULL,
					[CreatedAt] [datetime] NOT NULL,
					[Name] [varchar](100) NULL,
					[PhoneDDD] [varchar](2) NULL,
					[PhoneNumber] [varchar](9) NULL,
					[Email] [varchar](100) NULL,
				 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
				(
					[Id] ASC
				)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
				) ON [PRIMARY]
              END", connection);

            await createTableCommand.ExecuteNonQueryAsync();

            Console.WriteLine("✅ Tabela criada (ou já existia).");
        }
    }
}
