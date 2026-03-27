using Flashcards.Hillgrove.Menu.Actions.Stacks;
using Flashcards.Hillgrove.Models;
using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Menu.Actions.StudySessions
{
    internal class StartStudySessionAction : IStackAction
    {
        private readonly IStudySessionService _studySessionService;

        public StartStudySessionAction(IStudySessionService studySessionService)
        {
            _studySessionService = studySessionService;
        }

        public Task ExecuteAsync(Stack stack)
        {
            return _studySessionService.RunAsync(stack);
        }
    }
}
