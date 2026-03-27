using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Menu.Actions.StudySessions;
using Flashcards.Hillgrove.Menu.Items;
using Flashcards.Hillgrove.Menu.Items.Stacks;
using Flashcards.Hillgrove.Menu.Items.StudySessions;
using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Menu.Composition
{
    internal class StudySessionsSectionProvider : IMainMenuSectionProvider
    {
        private readonly IStackRepository _stackRepository;
        private readonly IStudySessionService _studySessionService;
        private readonly IAppUi _ui;
        private readonly IMenuPrompt _menuPrompt;

        public int Order => 3;

        public StudySessionsSectionProvider(
            IStackRepository stackRepository,
            IStudySessionService studySessionService,
            IAppUi ui,
            IMenuPrompt menuPrompt
        )
        {
            _stackRepository = stackRepository;
            _studySessionService = studySessionService;
            _ui = ui;
            _menuPrompt = menuPrompt;
        }

        public IMenuItem Create()
        {
            var startStudySessionAction = new StartStudySessionAction(_studySessionService);
            var viewStudySessionHistoryAction = new ViewStudySessionHistoryAction(_studySessionService, _ui);

            var startStudySession = new ShowAllStacksMenuItem(
                _stackRepository,
                startStudySessionAction,
                _ui,
                "Start Study Session",
                "[green]Choose a stack to study[/]"
            );
            var viewStudySessionHistory = new ViewStudySessionHistoryMenuItem(viewStudySessionHistoryAction);

            return new CompositeMenu(
                "Study Sessions",
                [startStudySession, viewStudySessionHistory, new BackMenuItem()],
                _menuPrompt
            );
        }
    }
}
