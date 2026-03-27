using System.Data;
using Dapper;
using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Data
{
    internal class StackRepository : IStackRepository
    {
        private readonly IDbConnection _connection;

        public StackRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Stack>> GetAllAsync()
        {
            var sql = "SELECT Id, Name FROM Stack";
            return await _connection.QueryAsync<Stack>(sql);
        }

        public async Task<Stack> AddAsync(Stack stack)
        {
            var sql =
                @"INSERT INTO Stack (Name) VALUES (@Name); 
                SELECT Id, Name FROM Stack WHERE Id = SCOPE_IDENTITY();";
            return await _connection.QuerySingleAsync<Stack>(sql, new { stack.Name });
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM Stack WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
