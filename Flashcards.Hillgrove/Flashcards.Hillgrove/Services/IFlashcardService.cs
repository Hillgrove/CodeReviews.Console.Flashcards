using Flashcards.Hillgrove.Dtos;

namespace Flashcards.Hillgrove.Services
{
    internal interface IFlashcardService
    {
        Task<IEnumerable<FlashCardDto>> GetByStackIdAsync(int stackId);
    }
}
