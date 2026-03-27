using Flashcards.Hillgrove.Menu.Actions.Reports;
using Flashcards.Hillgrove.Menu.Items;
using Flashcards.Hillgrove.Menu.Items.Reports;
using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Menu.Composition
{
    internal class ReportsSectionProvider : IMainMenuSectionProvider
    {
        private readonly IReportService _reportService;
        private readonly IAppUi _ui;
        private readonly IMenuPrompt _menuPrompt;

        public int Order => 4;

        public ReportsSectionProvider(IReportService reportService, IAppUi ui, IMenuPrompt menuPrompt)
        {
            _reportService = reportService;
            _ui = ui;
            _menuPrompt = menuPrompt;
        }

        public IMenuItem Create()
        {
            var viewSessionsAction = new ViewSessionsPerMonthPerStackAction(_reportService, _ui);
            var viewAverageScoreAction = new ViewAverageScorePerMonthPerStackAction(
                _reportService,
                _ui
            );

            var sessionsReport = new ViewSessionsPerMonthPerStackMenuItem(viewSessionsAction);
            var averageScoreReport = new ViewAverageScorePerMonthPerStackMenuItem(
                viewAverageScoreAction
            );

            return new CompositeMenu(
                "Reports",
                [sessionsReport, averageScoreReport, new BackMenuItem()],
                _menuPrompt
            );
        }
    }
}
