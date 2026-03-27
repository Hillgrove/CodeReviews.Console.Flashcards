using System.Data;
using Dapper;
using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Data
{
    internal class FlashcardRepository : IFlashcardRepository
    {
        private readonly IDbConnection _connection;

        public FlashcardRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Flashcard>> GetByStackIdAsync(int stackId)
        {
            var sql =
                "SELECT Id, Question, Answer FROM FlashCard WHERE StackId = @StackId ORDER BY Id";
            return await _connection.QueryAsync<Flashcard>(sql, new { StackId = stackId });
        }

        public async Task AddAsync(Flashcard card)
        {
            var sql =
                @"INSERT INTO FlashCard (StackId, Question, Answer) 
                  VALUES (@StackId, @Question, @Answer)";
            await _connection.ExecuteAsync(
                sql,
                new
                {
                    card.StackId,
                    card.Question,
                    card.Answer,
                }
            );
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM FlashCard WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
