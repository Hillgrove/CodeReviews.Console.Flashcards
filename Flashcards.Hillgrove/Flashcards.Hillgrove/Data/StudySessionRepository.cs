using System.Data;
using Dapper;
using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Data
{
    internal class StudySessionRepository : IStudySessionRepository
    {
        private readonly IDbConnection _connection;

        public StudySessionRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task AddAsync(StudySession studySession)
        {
            var sql =
                @"INSERT INTO StudySession (StackId, Date, Score)
                  VALUES (@StackId, @Date, @Score)";

            await _connection.ExecuteAsync(
                sql,
                new
                {
                    studySession.StackId,
                    studySession.Date,
                    studySession.Score,
                }
            );
        }

        public async Task<IEnumerable<StudySession>> GetAllAsync()
        {
            var sql =
                @"SELECT ss.Id, ss.StackId, s.Name AS StackName, ss.Date, ss.Score
                  FROM StudySession ss
                  INNER JOIN Stack s ON s.Id = ss.StackId
                  ORDER BY ss.Date DESC, ss.Id DESC";

            return await _connection.QueryAsync<StudySession>(sql);
        }
    }
}
