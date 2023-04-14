using System.Collections.Generic;
using System.Linq;
using Dapper;
using ProjectKanban.Data;
using ProjectKanban.Tasks;

namespace ProjectKanban.Users
{
    public sealed class UserRepository
    {
        private readonly IDatabase _database;

        public UserRepository(IDatabase database)
        {
            _database = database;
        }

        public void Create(UserRecord userRecord)
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                connection.Execute("INSERT INTO user (username, password, client_id) VALUES (@Username, @Password, @ClientId)", userRecord);
                transaction.Commit();
            }
        }

        public List<UserRecord> GetAll()
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                var users = connection.Query<UserRecord>("SELECT * from user ORDER BY username ASC;");
                return users.ToList();
            }
        }
    }
}