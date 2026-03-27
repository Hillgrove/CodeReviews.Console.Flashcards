using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Dtos;
using Flashcards.Hillgrove.Menu.Actions.Stacks;
using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Menu.Actions.Flashcards
{
    internal class DeleteFlashcardAction : IStackAction
    {
        private readonly IFlashcardRepository _repository;
        private readonly IAppUi _ui;

        public DeleteFlashcardAction(IFlashcardRepository repository, IAppUi ui)
        {
            _repository = repository;
            _ui = ui;
        }

        public async Task ExecuteAsync(Stack stack)
        {
            while (true)
            {
                _ui.Clear();

                var cards = (await _repository.GetByStackIdAsync(stack.Id)).ToList();

                if (cards.Count == 0)
                {
                    _ui.WriteWarning($"There are no flashcards in '{stack.Name}'.");
                    _ui.WaitForKey();
                    return;
                }

                _ui.WriteSuccess($"Delete flashcard from '{stack.Name}'\n");
                _ui.ShowFlashcardsTable($"{stack.Name} stack", ToDisplayCards(cards));

                var input = _ui
                    .PromptText("\nEnter flashcard # to delete (leave empty to cancel):", true)
                    .Trim();

                if (string.IsNullOrEmpty(input))
                {
                    return;
                }

                if (!int.TryParse(input, out var selectedIndex) || selectedIndex < 1 || selectedIndex > cards.Count)
                {
                    _ui.WriteError("Invalid flashcard number.");
                    _ui.WaitForKey();
                    continue;
                }

                var cardToDelete = cards[selectedIndex - 1];

                if (!_ui.Confirm($"Delete flashcard #{selectedIndex}?"))
                {
                    return;
                }

                await _repository.DeleteAsync(cardToDelete.Id);

                _ui.Clear();
                _ui.WriteSuccess($"Deleted flashcard #{selectedIndex}.\n");

                var updatedCards = (await _repository.GetByStackIdAsync(stack.Id)).ToList();
                _ui.ShowFlashcardsTable($"{stack.Name} stack", ToDisplayCards(updatedCards));
                _ui.WaitForKey();
                return;
            }
        }

        private static IEnumerable<FlashCardDto> ToDisplayCards(IReadOnlyList<Flashcard> cards)
        {
            return cards.Select(
                (card, index) =>
                    new FlashCardDto
                    {
                        DisplayIndex = index + 1,
                        Question = card.Question,
                        Answer = card.Answer,
                    }
            );
        }
    }
}
