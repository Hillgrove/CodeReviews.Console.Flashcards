using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Menu.Actions.Flashcards;
using Flashcards.Hillgrove.Menu.Actions.Stacks;
using Flashcards.Hillgrove.Menu.Items;
using Flashcards.Hillgrove.Menu.Items.Stacks;
using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Menu.Composition
{
    internal class ManageStacksSectionProvider : IMainMenuSectionProvider
    {
        private readonly IFlashcardRepository _flashcardRepository;
        private readonly IStackRepository _stackRepository;
        private readonly IStackService _stackService;
        private readonly IFlashcardService _flashcardService;
        private readonly IAppUi _ui;
        private readonly IMenuPrompt _menuPrompt;

        public int Order => 1;

        public ManageStacksSectionProvider(
            IFlashcardRepository flashcardRepository,
            IStackRepository stackRepository,
            IStackService stackService,
            IFlashcardService flashcardService,
            IAppUi ui,
            IMenuPrompt menuPrompt
        )
        {
            _flashcardRepository = flashcardRepository;
            _stackRepository = stackRepository;
            _stackService = stackService;
            _flashcardService = flashcardService;
            _ui = ui;
            _menuPrompt = menuPrompt;
        }

        public IMenuItem Create()
        {
            var showFlashcardsAction = new ShowFlashcardsAction(_flashcardService, _ui);
            var addFlashcardAction = new AddFlashCardAction(
                _flashcardRepository,
                showFlashcardsAction,
                _ui
            );
            var deleteStackAction = new DeleteStackAction(_stackRepository, _ui);

            var showStacks = new ShowAllStacksMenuItem(_stackRepository, showFlashcardsAction, _ui);
            var createStack = new CreateStackMenuItem(_stackService, addFlashcardAction, _ui);
            var deleteStack = new ShowAllStacksMenuItem(
                _stackRepository,
                deleteStackAction,
                _ui,
                "Delete a stack",
                "[green]Choose a stack to delete[/]"
            );

            return new CompositeMenu(
                "Manage Stacks",
                [showStacks, createStack, deleteStack, new BackMenuItem()],
                _menuPrompt
            );
        }
    }
}
