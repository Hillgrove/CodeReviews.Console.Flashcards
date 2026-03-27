using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Menu.Actions.Flashcards;
using Flashcards.Hillgrove.Menu.Items;
using Flashcards.Hillgrove.Menu.Items.Flashcards;
using Flashcards.Hillgrove.Menu.Items.Stacks;
using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Menu.Composition
{
    internal class ManageFlashcardsSectionProvider : IMainMenuSectionProvider
    {
        private readonly IStackRepository _stackRepository;
        private readonly IFlashcardRepository _flashcardRepository;
        private readonly IFlashcardService _flashcardService;
        private readonly IAppUi _ui;
        private readonly IMenuPrompt _menuPrompt;

        public int Order => 2;

        public ManageFlashcardsSectionProvider(
            IStackRepository stackRepository,
            IFlashcardRepository flashcardRepository,
            IFlashcardService flashcardService,
            IAppUi ui,
            IMenuPrompt menuPrompt
        )
        {
            _stackRepository = stackRepository;
            _flashcardRepository = flashcardRepository;
            _flashcardService = flashcardService;
            _ui = ui;
            _menuPrompt = menuPrompt;
        }

        public IMenuItem Create()
        {
            var showFlashcardsAction = new ShowFlashcardsAction(_flashcardService, _ui);
            var deleteFlashcardAction = new DeleteFlashcardAction(_flashcardRepository, _ui);

            var showStacks = new ShowAllStacksMenuItem(_stackRepository, showFlashcardsAction, _ui);
            var addFlashcard = new AddFlashcardToExistingStackMenuItem(_ui);
            var deleteFlashcard = new ShowAllStacksMenuItem(
                _stackRepository,
                deleteFlashcardAction,
                _ui,
                "Delete Flashcard from Stack",
                "[green]Choose a stack[/]"
            );

            return new CompositeMenu(
                "Manage Flashcards",
                [showStacks, addFlashcard, deleteFlashcard, new BackMenuItem()],
                _menuPrompt
            );
        }
    }
}
