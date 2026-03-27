using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Dtos;
using Flashcards.Hillgrove.Menu;
using Flashcards.Hillgrove.Models;
using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Tests.Services;

public class StudySessionServiceTests
{
    [Fact]
    public async Task RunAsync_SavesOneStudySessionWithCalculatedScore_WhenFlashcardsExist()
    {
        var flashcardRepository = new FakeFlashcardRepository(
            [
                new Flashcard { Id = 1, StackId = 8, Question = "Q1", Answer = "A1" },
                new Flashcard { Id = 2, StackId = 8, Question = "Q2", Answer = "A2" },
            ]
        );
        var studySessionRepository = new FakeStudySessionRepository();
        var ui = new FakeAppUi(promptInputs: ["a1", "wrong"]);
        var service = new StudySessionService(flashcardRepository, studySessionRepository, ui);
        var beforeRun = DateTime.UtcNow;

        await service.RunAsync(new Stack { Id = 8, Name = "Science" });

        Assert.Single(studySessionRepository.AddedSessions);
        var saved = studySessionRepository.AddedSessions[0];
        Assert.Equal(8, saved.StackId);
        Assert.Equal(1, saved.Score);
        Assert.True(saved.Date >= beforeRun);
        Assert.True(saved.Date <= DateTime.UtcNow.AddSeconds(1));
    }

    [Fact]
    public async Task RunAsync_DoesNotSaveStudySession_WhenNoFlashcardsExist()
    {
        var flashcardRepository = new FakeFlashcardRepository([]);
        var studySessionRepository = new FakeStudySessionRepository();
        var ui = new FakeAppUi(promptInputs: []);
        var service = new StudySessionService(flashcardRepository, studySessionRepository, ui);

        await service.RunAsync(new Stack { Id = 8, Name = "Science" });

        Assert.Empty(studySessionRepository.AddedSessions);
        Assert.Contains(ui.WarningMessages, message => message.Contains("There are no flashcards"));
    }

    [Fact]
    public async Task GetHistoryAsync_ReturnsSessionsFromRepository()
    {
        var expectedSessions = new[]
        {
            new StudySession { Id = 1, StackId = 8, Date = DateTime.UtcNow.AddDays(-1), Score = 3 },
            new StudySession { Id = 2, StackId = 9, Date = DateTime.UtcNow, Score = 5 },
        };
        var flashcardRepository = new FakeFlashcardRepository([]);
        var studySessionRepository = new FakeStudySessionRepository(historySessions: expectedSessions);
        var ui = new FakeAppUi(promptInputs: []);
        var service = new StudySessionService(flashcardRepository, studySessionRepository, ui);

        var history = await service.GetHistoryAsync();

        Assert.Equal(2, history.Count);
        Assert.Equal(expectedSessions.Select(s => s.Id), history.Select(s => s.Id));
    }

    private sealed class FakeFlashcardRepository(IEnumerable<Flashcard> cards) : IFlashcardRepository
    {
        private readonly List<Flashcard> _cards = cards.ToList();

        public Task<IEnumerable<Flashcard>> GetByStackIdAsync(int stackId)
        {
            return Task.FromResult(_cards.Where(c => c.StackId == stackId).AsEnumerable());
        }

        public Task AddAsync(Flashcard card) => Task.CompletedTask;

        public Task DeleteAsync(int id) => Task.CompletedTask;
    }

    private sealed class FakeStudySessionRepository(IEnumerable<StudySession>? historySessions = null)
        : IStudySessionRepository
    {
        private readonly List<StudySession> _historySessions = historySessions?.ToList() ?? [];

        public List<StudySession> AddedSessions { get; } = [];

        public Task AddAsync(StudySession studySession)
        {
            AddedSessions.Add(studySession);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<StudySession>> GetAllAsync()
        {
            return Task.FromResult(_historySessions.AsEnumerable());
        }
    }

    private sealed class FakeAppUi(IEnumerable<string> promptInputs) : IAppUi
    {
        private readonly Queue<string> _promptInputs = new(promptInputs);

        public List<string> WarningMessages { get; } = [];

        public void Clear() { }

        public void WriteSuccess(string message) { }

        public void WriteWarning(string message)
        {
            WarningMessages.Add(message);
        }

        public void WriteError(string message) { }

        public void WaitForKey(string message = "[grey]Press any key to continue...[/]") { }

        public string PromptText(string prompt, bool allowEmpty = false)
        {
            return _promptInputs.Count > 0 ? _promptInputs.Dequeue() : string.Empty;
        }

        public bool Confirm(string prompt) => false;

        public Stack PromptStackSelection(string title, IReadOnlyList<Stack> stacks)
        {
            return stacks.FirstOrDefault() ?? new Stack { Name = string.Empty };
        }

        public void ShowFlashcardsTable(string title, IEnumerable<FlashCardDto> cards) { }

        public void ShowStudySessionsTable(string title, IEnumerable<StudySession> sessions) { }

        public void ShowStackReportTable(string title, IEnumerable<StackReportRow> rows) { }
    }
}
