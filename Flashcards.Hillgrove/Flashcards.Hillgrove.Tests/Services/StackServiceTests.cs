using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Models;
using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Tests.Services;

public class StackServiceTests
{
    [Fact]
    public async Task AddAsync_ReturnsNull_WhenStackNameAlreadyExists()
    {
        var existingStack = new Stack { Id = 1, Name = "Geography" };
        var repository = new FakeStackRepository { AllStacks = [existingStack] };

        var service = new StackService(repository);

        var result = await service.AddAsync(new Stack { Name = "Geography" });

        Assert.Null(result);
        Assert.False(repository.AddCalled);
    }

    [Fact]
    public async Task AddAsync_ReturnsAddedStack_WhenStackNameIsUnique()
    {
        var repository = new FakeStackRepository
        {
            AllStacks = [new Stack { Id = 1, Name = "Math" }],
            StackToReturnFromAdd = new Stack { Id = 2, Name = "Science" },
        };

        var service = new StackService(repository);

        var result = await service.AddAsync(new Stack { Name = "Science" });

        Assert.NotNull(result);
        Assert.True(repository.AddCalled);
        Assert.Equal("Science", result!.Name);
        Assert.Equal(2, result.Id);
    }

    private sealed class FakeStackRepository : IStackRepository
    {
        public IEnumerable<Stack> AllStacks { get; set; } = [];
        public Stack StackToReturnFromAdd { get; set; } = new Stack { Id = 1, Name = "Default" };
        public bool AddCalled { get; private set; }

        public Task<IEnumerable<Stack>> GetAllAsync() => Task.FromResult(AllStacks);

        public Task<Stack> AddAsync(Stack stack)
        {
            AddCalled = true;
            return Task.FromResult(StackToReturnFromAdd);
        }

        public Task DeleteAsync(int id) => Task.CompletedTask;
    }
}
