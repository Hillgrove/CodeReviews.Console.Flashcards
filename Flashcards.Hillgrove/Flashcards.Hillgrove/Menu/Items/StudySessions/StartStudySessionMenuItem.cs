using Flashcards.Hillgrove.Helpers;

namespace Flashcards.Hillgrove.Menu.Items.StudySessions
{
    internal class StartStudySessionMenuItem : IMenuItem
    {
        private readonly IAppUi _ui;

        public string Label => "Start Study Session";

        public StartStudySessionMenuItem(IAppUi ui)
        {
            _ui = ui;
        }

        public Task<NavigationResult> ExecuteAsync()
        {
            _ui.WriteWarning("Start Study Session is not implemented yet.");
            _ui.WaitForKey();
            return Task.FromResult(NavigationResult.Continue);
        }
    }
}
