using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Dtos;
using Flashcards.Hillgrove.Menu;
using Flashcards.Hillgrove.Menu.Actions.Flashcards;
using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Tests.Menu.Actions.Flashcards;

public class DeleteFlashcardActionTests
{
    [Fact]
    public async Task ExecuteAsync_DeletesCardByMappedDisplayIndex_WhenSelectionIsValid()
    {
        var repository = new FakeFlashcardRepository(
            [
                new Flashcard { Id = 11, StackId = 3, Question = "Q1", Answer = "A1" },
                new Flashcard { Id = 25, StackId = 3, Question = "Q2", Answer = "A2" },
            ]
        );
        var ui = new FakeAppUi(promptInputs: ["2"], confirmResults: [true]);
        var action = new DeleteFlashcardAction(repository, ui);

        await action.ExecuteAsync(new Stack { Id = 3, Name = "Math" });

        Assert.Equal([25], repository.DeletedIds);
        Assert.Equal(2, ui.ShownTables.Count);
        Assert.Equal([1, 2], ui.ShownTables[0].Select(c => c.DisplayIndex).ToArray());
        Assert.Equal([1], ui.ShownTables[1].Select(c => c.DisplayIndex).ToArray());
    }

    [Fact]
    public async Task ExecuteAsync_DoesNotDelete_WhenSelectedDisplayIndexIsInvalid()
    {
        var repository = new FakeFlashcardRepository(
            [new Flashcard { Id = 11, StackId = 3, Question = "Q1", Answer = "A1" }]
        );
        var ui = new FakeAppUi(promptInputs: ["9", ""], confirmResults: []);
        var action = new DeleteFlashcardAction(repository, ui);

        await action.ExecuteAsync(new Stack { Id = 3, Name = "Math" });

        Assert.Empty(repository.DeletedIds);
        Assert.Contains(ui.ErrorMessages, message => message.Contains("Invalid flashcard number."));
    }

    [Fact]
    public async Task ExecuteAsync_ShowsWarning_WhenStackHasNoCards()
    {
        var repository = new FakeFlashcardRepository([]);
        var ui = new FakeAppUi(promptInputs: [], confirmResults: []);
        var action = new DeleteFlashcardAction(repository, ui);

        await action.ExecuteAsync(new Stack { Id = 3, Name = "Math" });

        Assert.Empty(repository.DeletedIds);
        Assert.Contains(ui.WarningMessages, message => message.Contains("There are no flashcards"));
        Assert.Equal(1, ui.WaitForKeyCalls);
    }

    private sealed class FakeFlashcardRepository(IEnumerable<Flashcard> cards) : IFlashcardRepository
    {
        private readonly List<Flashcard> _cards = cards.ToList();

        public List<int> DeletedIds { get; } = [];

        public Task<IEnumerable<Flashcard>> GetByStackIdAsync(int stackId)
        {
            return Task.FromResult(_cards.Where(card => card.StackId == stackId).AsEnumerable());
        }

        public Task AddAsync(Flashcard card)
        {
            _cards.Add(card);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            DeletedIds.Add(id);
            _cards.RemoveAll(card => card.Id == id);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeAppUi(IEnumerable<string> promptInputs, IEnumerable<bool> confirmResults)
        : IAppUi
    {
        private readonly Queue<string> _promptInputs = new(promptInputs);
        private readonly Queue<bool> _confirmResults = new(confirmResults);

        public List<string> WarningMessages { get; } = [];
        public List<string> ErrorMessages { get; } = [];
        public List<List<FlashCardDto>> ShownTables { get; } = [];
        public int WaitForKeyCalls { get; private set; }

        public void Clear() { }

        public void WriteSuccess(string message) { }

        public void WriteWarning(string message)
        {
            WarningMessages.Add(message);
        }

        public void WriteError(string message)
        {
            ErrorMessages.Add(message);
        }

        public void WaitForKey(string message = "[grey]Press any key to continue...[/]")
        {
            WaitForKeyCalls++;
        }

        public string PromptText(string prompt, bool allowEmpty = false)
        {
            return _promptInputs.Count > 0 ? _promptInputs.Dequeue() : string.Empty;
        }

        public bool Confirm(string prompt)
        {
            return _confirmResults.Count > 0 && _confirmResults.Dequeue();
        }

        public Stack PromptStackSelection(string title, IReadOnlyList<Stack> stacks)
        {
            return stacks.First();
        }

        public void ShowFlashcardsTable(string title, IEnumerable<FlashCardDto> cards)
        {
            ShownTables.Add(cards.ToList());
        }

        public void ShowStudySessionsTable(string title, IEnumerable<StudySession> sessions) { }

        public void ShowStackReportTable(string title, IEnumerable<StackReportRow> rows) { }
    }
}
