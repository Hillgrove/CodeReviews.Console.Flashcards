using Flashcards.Hillgrove.Menu.Actions.Stacks;
using Flashcards.Hillgrove.Models;
using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Menu.Actions.Flashcards
{
    internal class ShowFlashcardsAction : IStackAction
    {
        private readonly IFlashcardService _service;
        private readonly IAppUi _ui;

        public ShowFlashcardsAction(IFlashcardService service, IAppUi ui)
        {
            _service = service;
            _ui = ui;
        }

        public async Task ExecuteAsync(Stack stack)
        {
            var cards = await _service.GetByStackIdAsync(stack.Id);
            _ui.ShowFlashcardsTable($"{stack.Name} stack", cards);
            _ui.WaitForKey();
        }
    }
}
