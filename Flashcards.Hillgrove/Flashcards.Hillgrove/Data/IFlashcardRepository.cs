using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Data
{
    internal interface IFlashcardRepository
    {
        Task<IEnumerable<Flashcard>> GetByStackIdAsync(int stackId);
        Task AddAsync(Flashcard card);
        Task DeleteAsync(int id);
    }
}
