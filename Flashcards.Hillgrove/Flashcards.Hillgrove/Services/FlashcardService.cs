using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Dtos;

namespace Flashcards.Hillgrove.Services
{
    internal class FlashcardService : IFlashcardService
    {
        private readonly IFlashcardRepository _repository;

        public FlashcardService(IFlashcardRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FlashCardDto>> GetByStackIdAsync(int stackId)
        {
            var foundCards = await _repository.GetByStackIdAsync(stackId);

            var cards = foundCards.Select(
                (fc, index) =>
                    new FlashCardDto
                    {
                        DisplayIndex = index + 1,
                        Question = fc.Question,
                        Answer = fc.Answer,
                    }
            );

            return cards;
        }
    }
}
