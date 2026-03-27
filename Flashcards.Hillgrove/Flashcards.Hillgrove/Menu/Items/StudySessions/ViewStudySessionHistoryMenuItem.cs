using Flashcards.Hillgrove.Helpers;
using Flashcards.Hillgrove.Menu.Actions.StudySessions;

namespace Flashcards.Hillgrove.Menu.Items.StudySessions
{
    internal class ViewStudySessionHistoryMenuItem : IMenuItem
    {
        private readonly ViewStudySessionHistoryAction _viewStudySessionHistoryAction;

        public string Label => "View Study Session History";

        public ViewStudySessionHistoryMenuItem(ViewStudySessionHistoryAction viewStudySessionHistoryAction)
        {
            _viewStudySessionHistoryAction = viewStudySessionHistoryAction;
        }

        public async Task<NavigationResult> ExecuteAsync()
        {
            await _viewStudySessionHistoryAction.ExecuteAsync();
            return NavigationResult.Continue;
        }
    }
}
