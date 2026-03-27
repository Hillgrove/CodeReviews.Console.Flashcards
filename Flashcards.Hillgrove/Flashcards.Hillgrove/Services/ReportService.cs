using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Services
{
    internal class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<IReadOnlyList<StackReportRow>> GetSessionsPerMonthPerStackAsync(int year)
        {
            var reportRows = await _reportRepository.GetSessionsPerMonthPerStackAsync(year);
            return reportRows.ToList();
        }

        public async Task<IReadOnlyList<StackReportRow>> GetAverageScorePerMonthPerStackAsync(int year)
        {
            var reportRows = await _reportRepository.GetAverageScorePerMonthPerStackAsync(year);
            return reportRows.ToList();
        }
    }
}
