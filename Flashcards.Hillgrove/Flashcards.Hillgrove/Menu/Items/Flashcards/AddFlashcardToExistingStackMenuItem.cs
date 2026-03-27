using Flashcards.Hillgrove.Helpers;

namespace Flashcards.Hillgrove.Menu.Items.Flashcards
{
    internal class AddFlashcardToExistingStackMenuItem : IMenuItem
    {
        private readonly IAppUi _ui;

        public string Label => "Add Flashcard to Existing Stack";

        public AddFlashcardToExistingStackMenuItem(IAppUi ui)
        {
            _ui = ui;
        }

        public Task<NavigationResult> ExecuteAsync()
        {
            _ui.WriteWarning("Add Flashcard to Existing Stack is not implemented yet.");
            _ui.WaitForKey();
            return Task.FromResult(NavigationResult.Continue);
        }
    }
}
