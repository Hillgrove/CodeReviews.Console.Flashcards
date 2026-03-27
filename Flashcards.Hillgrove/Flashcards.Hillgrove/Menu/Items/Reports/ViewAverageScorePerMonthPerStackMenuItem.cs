using Flashcards.Hillgrove.Helpers;
using Flashcards.Hillgrove.Menu.Actions.Reports;

namespace Flashcards.Hillgrove.Menu.Items.Reports
{
    internal class ViewAverageScorePerMonthPerStackMenuItem : IMenuItem
    {
        private readonly ViewAverageScorePerMonthPerStackAction _action;

        public string Label => "Average Score per Month per Stack";

        public ViewAverageScorePerMonthPerStackMenuItem(ViewAverageScorePerMonthPerStackAction action)
        {
            _action = action;
        }

        public async Task<NavigationResult> ExecuteAsync()
        {
            await _action.ExecuteAsync();
            return NavigationResult.Continue;
        }
    }
}
