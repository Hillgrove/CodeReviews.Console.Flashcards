using Flashcards.Hillgrove.Dtos;
using Flashcards.Hillgrove.Menu;
using Flashcards.Hillgrove.Menu.Actions.StudySessions;
using Flashcards.Hillgrove.Models;
using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Tests.Menu.Actions.StudySessions;

public class ViewStudySessionHistoryActionTests
{
    [Fact]
    public async Task ExecuteAsync_ShowsHistoryTable_WhenSessionsExist()
    {
        var sessions = new[]
        {
            new StudySession { Id = 1, StackId = 3, Date = DateTime.UtcNow, Score = 2 },
            new StudySession { Id = 2, StackId = 4, Date = DateTime.UtcNow.AddDays(-1), Score = 4 },
        };
        var service = new FakeStudySessionService(sessions);
        var ui = new FakeAppUi();
        var action = new ViewStudySessionHistoryAction(service, ui);

        await action.ExecuteAsync();

        Assert.True(ui.HistoryShown);
        Assert.Equal(2, ui.ShownSessions.Count);
        Assert.Equal(1, ui.WaitForKeyCalls);
        Assert.Empty(ui.WarningMessages);
    }

    [Fact]
    public async Task ExecuteAsync_ShowsWarning_WhenNoSessionsExist()
    {
        var service = new FakeStudySessionService([]);
        var ui = new FakeAppUi();
        var action = new ViewStudySessionHistoryAction(service, ui);

        await action.ExecuteAsync();

        Assert.False(ui.HistoryShown);
        Assert.Contains(ui.WarningMessages, message => message.Contains("There are no study sessions"));
        Assert.Equal(1, ui.WaitForKeyCalls);
    }

    private sealed class FakeStudySessionService(IEnumerable<StudySession> sessions) : IStudySessionService
    {
        public Task RunAsync(Stack stack) => Task.CompletedTask;

        public Task<IReadOnlyList<StudySession>> GetHistoryAsync()
        {
            return Task.FromResult<IReadOnlyList<StudySession>>(sessions.ToList());
        }
    }

    private sealed class FakeAppUi : IAppUi
    {
        public bool HistoryShown { get; private set; }
        public List<StudySession> ShownSessions { get; } = [];
        public List<string> WarningMessages { get; } = [];
        public int WaitForKeyCalls { get; private set; }

        public void Clear() { }

        public void WriteSuccess(string message) { }

        public void WriteWarning(string message)
        {
            WarningMessages.Add(message);
        }

        public void WriteError(string message) { }

        public void WaitForKey(string message = "[grey]Press any key to continue...[/]")
        {
            WaitForKeyCalls++;
        }

        public string PromptText(string prompt, bool allowEmpty = false) => string.Empty;

        public bool Confirm(string prompt) => false;

        public Stack PromptStackSelection(string title, IReadOnlyList<Stack> stacks) =>
            stacks.FirstOrDefault() ?? new Stack { Name = string.Empty };

        public void ShowFlashcardsTable(string title, IEnumerable<FlashCardDto> cards) { }

        public void ShowStudySessionsTable(string title, IEnumerable<StudySession> sessions)
        {
            HistoryShown = true;
            ShownSessions.AddRange(sessions);
        }

        public void ShowStackReportTable(string title, IEnumerable<StackReportRow> rows) { }
    }
}
