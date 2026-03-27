using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Models;
using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Tests.Services;

public class FlashcardServiceTests
{
    [Fact]
    public async Task GetByStackIdAsync_ReturnsMappedDtos_WhenRepositoryContainsCards()
    {
        var repository = new FakeFlashcardRepository
        {
            CardsByStackId = new Dictionary<int, IEnumerable<Flashcard>>
            {
                [5] =
                [
                    new Flashcard { Id = 1, StackId = 5, Question = "Q1", Answer = "A1" },
                    new Flashcard { Id = 2, StackId = 5, Question = "Q2", Answer = "A2" },
                ],
            },
        };

        var service = new FlashcardService(repository);

        var result = (await service.GetByStackIdAsync(5)).ToList();

        Assert.Equal(5, repository.LastRequestedStackId);
        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].DisplayIndex);
        Assert.Equal("Q1", result[0].Question);
        Assert.Equal("A1", result[0].Answer);
        Assert.Equal(2, result[1].DisplayIndex);
        Assert.Equal("Q2", result[1].Question);
        Assert.Equal("A2", result[1].Answer);
    }

    [Fact]
    public async Task GetByStackIdAsync_ReturnsContiguousDisplayIndexes_WhenSourceIdsHaveGaps()
    {
        var repository = new FakeFlashcardRepository
        {
            CardsByStackId = new Dictionary<int, IEnumerable<Flashcard>>
            {
                [10] =
                [
                    new Flashcard { Id = 1, StackId = 10, Question = "Q1", Answer = "A1" },
                    new Flashcard { Id = 3, StackId = 10, Question = "Q3", Answer = "A3" },
                ],
            },
        };

        var service = new FlashcardService(repository);

        var result = (await service.GetByStackIdAsync(10)).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].DisplayIndex);
        Assert.Equal(2, result[1].DisplayIndex);
    }

    [Fact]
    public async Task GetByStackIdAsync_ReturnsEmptyCollection_WhenRepositoryHasNoCards()
    {
        var repository = new FakeFlashcardRepository();
        var service = new FlashcardService(repository);

        var result = await service.GetByStackIdAsync(999);

        Assert.Equal(999, repository.LastRequestedStackId);
        Assert.Empty(result);
    }

    private sealed class FakeFlashcardRepository : IFlashcardRepository
    {
        public Dictionary<int, IEnumerable<Flashcard>> CardsByStackId { get; set; } = [];
        public int? LastRequestedStackId { get; private set; }

        public Task<IEnumerable<Flashcard>> GetByStackIdAsync(int stackId)
        {
            LastRequestedStackId = stackId;
            CardsByStackId.TryGetValue(stackId, out var cards);
            return Task.FromResult(cards ?? Enumerable.Empty<Flashcard>());
        }

        public Task AddAsync(Flashcard card) => Task.CompletedTask;

        public Task DeleteAsync(int id) => Task.CompletedTask;
    }
}
