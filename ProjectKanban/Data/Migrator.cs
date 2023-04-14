using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Dapper;

namespace ProjectKanban.Data
{
    public sealed class Migrator
    {
        private string ReadEmbeddedResource(Assembly assembly, string resourceIdentifier)
        {
            using var reader = new StreamReader(assembly.GetManifestResourceStream(resourceIdentifier));
            return reader.ReadToEnd();
        }

        public void Migrate(IDatabase database)
        {
            var assembly = typeof(Migrator).Assembly;
            var migrations = assembly
                .GetManifestResourceNames()
                .OrderBy(x => x).ToList();

            using (var connection = database.Connect())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                foreach (var migration in migrations)
                {
                    var command = ReadEmbeddedResource(assembly, migration);
                    try
                    {
                        connection.Execute(command);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(command, e);
                    }
                }

                transaction.Commit();
            }
        }
    }
}