using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Menu.Actions.Reports
{
    internal class ViewAverageScorePerMonthPerStackAction
    {
        private readonly IReportService _reportService;
        private readonly IAppUi _ui;

        public ViewAverageScorePerMonthPerStackAction(IReportService reportService, IAppUi ui)
        {
            _reportService = reportService;
            _ui = ui;
        }

        public async Task ExecuteAsync()
        {
            _ui.Clear();
            _ui.WriteSuccess("Average Score per Month per Stack\n");

            var year = PromptYear();

            if (year is null)
            {
                return;
            }

            var reportRows = await _reportService.GetAverageScorePerMonthPerStackAsync(year.Value);

            if (reportRows.Count == 0)
            {
                _ui.WriteWarning($"No report data found for {year}.");
                _ui.WaitForKey();
                return;
            }

            _ui.ShowStackReportTable($"Average Score per Month per Stack ({year})", reportRows);
            _ui.WaitForKey();
        }

        private int? PromptYear()
        {
            while (true)
            {
                var input = _ui.PromptText(
                        "Enter year [grey](leave blank to cancel)[/]",
                        allowEmpty: true
                    )
                    .Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    return null;
                }

                if (int.TryParse(input, out var year) && year is >= 1 and <= 9999)
                {
                    return year;
                }

                _ui.WriteWarning("Please enter a valid year.");
            }
        }
    }
}
