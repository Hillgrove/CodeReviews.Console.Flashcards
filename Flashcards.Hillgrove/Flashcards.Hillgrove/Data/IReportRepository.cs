using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Data
{
    internal interface IReportRepository
    {
        Task<IEnumerable<StackReportRow>> GetSessionsPerMonthPerStackAsync(int year);
        Task<IEnumerable<StackReportRow>> GetAverageScorePerMonthPerStackAsync(int year);
    }
}
