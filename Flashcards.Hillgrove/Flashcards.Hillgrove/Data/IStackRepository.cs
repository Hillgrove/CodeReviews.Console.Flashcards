using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Data
{
    internal interface IStackRepository
    {
        Task<IEnumerable<Stack>> GetAllAsync();
        Task<Stack> AddAsync(Stack stack);
        Task DeleteAsync(int id);
    }
}
