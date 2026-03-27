using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Services
{
    internal class StackService : IStackService
    {
        private readonly IStackRepository _repository;

        public StackService(IStackRepository repository)
        {
            _repository = repository;
        }

        public async Task<Stack?> AddAsync(Stack newStack)
        {
            IEnumerable<Stack> existingStacks = await _repository.GetAllAsync();
            bool stackNameExist = existingStacks.Any(s => s.Name == newStack.Name);

            if (stackNameExist)
            {
                return null;
            }

            return await _repository.AddAsync(newStack);
        }
    }
}
