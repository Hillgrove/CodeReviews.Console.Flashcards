using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Menu.Actions.Stacks;
using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Menu.Actions.Flashcards
{
    internal class AddFlashCardAction : IStackAction
    {
        private readonly IFlashcardRepository _repository;
        private readonly IStackAction _onCompleted;
        private readonly IAppUi _ui;

        public AddFlashCardAction(IFlashcardRepository repository, IStackAction onCreated, IAppUi ui)
        {
            _repository = repository;
            _onCompleted = onCreated;
            _ui = ui;
        }

        public async Task ExecuteAsync(Stack stack)
        {
            while (true)
            {
                _ui.Clear();
                _ui.WriteSuccess($"Add flashcards to '{stack.Name}'\n");

                var question = _ui.PromptText("Enter question:").Trim();
                var answer = _ui.PromptText("Enter answer:").Trim();

                if (!_ui.Confirm("\nSave this card?"))
                    continue;

                var card = new Flashcard
                {
                    Question = question,
                    Answer = answer,
                    StackId = stack.Id,
                };

                await _repository.AddAsync(card);

                _ui.WriteSuccess("\nFlashcard added!");

                if (!_ui.Confirm("\n\nAdd another card to this stack?"))
                    break;
            }

            _ui.Clear();
            await _onCompleted.ExecuteAsync(stack);
        }
    }
}
