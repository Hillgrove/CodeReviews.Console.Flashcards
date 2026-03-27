using Flashcards.Hillgrove.Helpers;
using Flashcards.Hillgrove.Menu.Actions.Reports;

namespace Flashcards.Hillgrove.Menu.Items.Reports
{
    internal class ViewSessionsPerMonthPerStackMenuItem : IMenuItem
    {
        private readonly ViewSessionsPerMonthPerStackAction _action;

        public string Label => "Sessions per Month per Stack";

        public ViewSessionsPerMonthPerStackMenuItem(ViewSessionsPerMonthPerStackAction action)
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
