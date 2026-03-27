using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Menu.Actions.StudySessions
{
    internal class ViewStudySessionHistoryAction
    {
        private readonly IStudySessionService _studySessionService;
        private readonly IAppUi _ui;

        public ViewStudySessionHistoryAction(IStudySessionService studySessionService, IAppUi ui)
        {
            _studySessionService = studySessionService;
            _ui = ui;
        }

        public async Task ExecuteAsync()
        {
            var sessions = await _studySessionService.GetHistoryAsync();

            if (sessions.Count == 0)
            {
                _ui.WriteWarning("There are no study sessions available.");
                _ui.WaitForKey();
                return;
            }

            _ui.ShowStudySessionsTable("Study Session History", sessions);
            _ui.WaitForKey();
        }
    }
}
