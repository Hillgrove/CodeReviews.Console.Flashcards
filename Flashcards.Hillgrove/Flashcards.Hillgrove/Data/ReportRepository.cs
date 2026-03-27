using System.Data;
using Dapper;
using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Data
{
    internal class ReportRepository : IReportRepository
    {
        private readonly IDbConnection _connection;

        public ReportRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<StackReportRow>> GetSessionsPerMonthPerStackAsync(int year)
        {
            var sql =
                @"
                SELECT 
                    StackName,
                    ISNULL(CAST([1] AS INT), 0) AS January,
                    ISNULL(CAST([2] AS INT), 0) AS February,
                    ISNULL(CAST([3] AS INT), 0) AS March,
                    ISNULL(CAST([4] AS INT), 0) AS April,
                    ISNULL(CAST([5] AS INT), 0) AS May,
                    ISNULL(CAST([6] AS INT), 0) AS June,
                    ISNULL(CAST([7] AS INT), 0) AS July,
                    ISNULL(CAST([8] AS INT), 0) AS August,
                    ISNULL(CAST([9] AS INT), 0) AS September,
                    ISNULL(CAST([10] AS INT), 0) AS October,
                    ISNULL(CAST([11] AS INT), 0) AS November,
                    ISNULL(CAST([12] AS INT), 0) AS December
                FROM (
                    SELECT 
                        s.Name AS StackName,
                        MONTH(ss.Date) AS Month,
                        COUNT(ss.Id) AS SessionCount
                    FROM Stack s
                    LEFT JOIN StudySession ss ON s.Id = ss.StackId AND YEAR(ss.Date) = @Year
                    GROUP BY s.Name, MONTH(ss.Date)
                ) SourceTable
                PIVOT (
                    SUM(SessionCount)
                    FOR [Month] IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                ) PivotTable
                ORDER BY StackName";

            return await _connection.QueryAsync<StackReportRow>(sql, new { Year = year });
        }

        public async Task<IEnumerable<StackReportRow>> GetAverageScorePerMonthPerStackAsync(
            int year
        )
        {
            var sql =
                @"
                SELECT 
                    StackName,
                    ISNULL(ROUND([1], 2), 0) AS January,
                    ISNULL(ROUND([2], 2), 0) AS February,
                    ISNULL(ROUND([3], 2), 0) AS March,
                    ISNULL(ROUND([4], 2), 0) AS April,
                    ISNULL(ROUND([5], 2), 0) AS May,
                    ISNULL(ROUND([6], 2), 0) AS June,
                    ISNULL(ROUND([7], 2), 0) AS July,
                    ISNULL(ROUND([8], 2), 0) AS August,
                    ISNULL(ROUND([9], 2), 0) AS September,
                    ISNULL(ROUND([10], 2), 0) AS October,
                    ISNULL(ROUND([11], 2), 0) AS November,
                    ISNULL(ROUND([12], 2), 0) AS December
                    FROM (
                        SELECT 
                            s.Name AS StackName,
                            MONTH(ss.Date) AS Month,
                            AVG(CAST(ss.Score AS FLOAT)) AS AverageScore
                        FROM Stack s
                        LEFT JOIN StudySession ss ON s.Id = ss.StackId AND YEAR(ss.Date) = @Year
                        GROUP BY s.Name, MONTH(ss.Date)
                    ) SourceTable
                PIVOT (
                    AVG(AverageScore)
                    FOR [Month] IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
                ) PivotTable
                ORDER BY StackName";

            return await _connection.QueryAsync<StackReportRow>(sql, new { Year = year });
        }
    }
}
