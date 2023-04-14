using Dapper;
using ProjectKanban.Data;
using ProjectKanban.Tasks;

namespace ProjectKanban.Users
{
    public sealed class ClientsRepository
    {
        private readonly IDatabase _database;

        public ClientsRepository(IDatabase database)
        {
            _database = database;
        }

        public ClientRecord Create(ClientRecord clientRecord)
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                clientRecord.Id = connection.Insert("INSERT INTO client (name) VALUES (@Name);", clientRecord);
                transaction.Commit();
                return clientRecord;
            }
        }
    }
}