using System;
using System.Data;
using System.Threading;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectKanban.Data
{
    public interface IDatabase
    {
        IDbConnection Connect();
    }

    public class SqliteMemoryDatabase : IDatabase
    {
        private readonly SqlLiteTestingConnection _connection;

        public SqliteMemoryDatabase()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connection = new SqlLiteTestingConnection("Data Source=:memory:");
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IDatabase>(this);
            new Migrator().Migrate(this);
        }

        public IDbConnection Connect()
        {
            return _connection;
        }

        public string GetMigrationsFolderName()
        {
            return "Sqlite";
        }

        public void Dispose()
        {
            _connection.ActuallyDispose();
        }

        public class SqlLiteTestingConnection : SqliteConnection, IDisposable
        {
            public SqlLiteTestingConnection(string connectionString) : base(connectionString)
            {
            }

            public new void Dispose()
            {
            }

            public void ActuallyDispose()
            {
                base.Dispose();
            }

            public override void Close()
            {
                base.Close();
            }
        }
    }

    public static class OrmExtensions
    {
        public static int Insert(this IDbConnection connection, string sqlQuery, object param)
        {
            if (!sqlQuery.Contains(";"))
                throw new Exception("Queries must end in a semi colon ;");

            return connection.ExecuteScalar<int>(sqlQuery.Replace(";", "; select last_insert_rowid();"), param);
        }
    }
}