using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Services
{
    internal interface IStackService
    {
        Task<Stack?> AddAsync(Stack newStack);
    }
}
