using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Services
{
    internal interface IReportService
    {
        Task<IReadOnlyList<StackReportRow>> GetSessionsPerMonthPerStackAsync(int year);
        Task<IReadOnlyList<StackReportRow>> GetAverageScorePerMonthPerStackAsync(int year);
    }
}
