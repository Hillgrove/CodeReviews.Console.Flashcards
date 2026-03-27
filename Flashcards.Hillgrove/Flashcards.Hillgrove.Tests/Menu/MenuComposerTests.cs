using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Dtos;
using Flashcards.Hillgrove.Helpers;
using Flashcards.Hillgrove.Menu;
using Flashcards.Hillgrove.Menu.Composition;
using Flashcards.Hillgrove.Models;
using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Tests.Menu;

public class MenuComposerTests
{
    [Fact]
    public void Build_ComposesMenuFromProvidersInOrder_WhenProvidersAreRegistered()
    {
        var composer = new MenuComposer(
            [
                new FakeMainMenuSectionProvider("Second", 2),
                new FakeMainMenuSectionProvider("First", 1),
            ],
            new FakeMenuPrompt()
        );

        var menu = composer.Build();

        var root = Assert.IsType<CompositeMenu>(menu);
        var labels = root.Items.Select(i => i.Label).ToArray();

        Assert.Equal(["First", "Second"], labels);
    }

    [Fact]
    public void Build_IncludesNewProviderContribution_WithoutChangingComposer()
    {
        var composer = new MenuComposer(
            [
                new FakeMainMenuSectionProvider("Manage Stacks", 1),
                new FakeMainMenuSectionProvider("Custom Reports", 2),
            ],
            new FakeMenuPrompt()
        );

        var menu = composer.Build();

        var root = Assert.IsType<CompositeMenu>(menu);
        Assert.Contains(root.Items, i => i.Label == "Custom Reports");
    }

    [Fact]
    public void Build_ReturnsMainMenuWithExpectedTopLevelLabels_WhenComposingDefaultProviders()
    {
        var menu = BuildMenuFromDefaultProviders();

        var root = Assert.IsType<CompositeMenu>(menu);
        var labels = root.Items.Select(i => i.Label).ToArray();

        Assert.Equal(["Manage Stacks", "Manage Flashcards", "Study Sessions", "Reports", "Exit"], labels);
    }

    [Fact]
    public void Build_ReturnsManageStacksMenuWithExpectedItemsInOrder_WhenComposingDefaultProviders()
    {
        var menu = BuildMenuFromDefaultProviders();

        var root = Assert.IsType<CompositeMenu>(menu);
        var manageStacks = Assert.IsType<CompositeMenu>(
            root.Items.Single(i => i.Label == "Manage Stacks")
        );
        var labels = manageStacks.Items.Select(i => i.Label).ToArray();

        Assert.Equal(["Show All Stacks", "Create a new stack", "Delete a stack", "Back"], labels);
    }

    [Fact]
    public void Build_ReturnsManageFlashcardsMenuWithExpectedItemsInOrder_WhenComposingDefaultProviders()
    {
        var menu = BuildMenuFromDefaultProviders();

        var root = Assert.IsType<CompositeMenu>(menu);
        var manageFlashcards = Assert.IsType<CompositeMenu>(
            root.Items.Single(i => i.Label == "Manage Flashcards")
        );
        var labels = manageFlashcards.Items.Select(i => i.Label).ToArray();

        Assert.Equal(
            [
                "Show All Stacks",
                "Add Flashcard to Existing Stack",
                "Delete Flashcard from Stack",
                "Back",
            ],
            labels
        );
    }

    [Fact]
    public void Build_ReturnsStudySessionsMenuWithExpectedItemsInOrder_WhenComposingDefaultProviders()
    {
        var menu = BuildMenuFromDefaultProviders();

        var root = Assert.IsType<CompositeMenu>(menu);
        var studySessions = Assert.IsType<CompositeMenu>(
            root.Items.Single(i => i.Label == "Study Sessions")
        );
        var labels = studySessions.Items.Select(i => i.Label).ToArray();

        Assert.Equal(["Start Study Session", "View Study Session History", "Back"], labels);
    }

    [Fact]
    public void Build_ReturnsReportsMenuWithExpectedItemsInOrder_WhenComposingDefaultProviders()
    {
        var menu = BuildMenuFromDefaultProviders();

        var root = Assert.IsType<CompositeMenu>(menu);
        var reports = Assert.IsType<CompositeMenu>(root.Items.Single(i => i.Label == "Reports"));
        var labels = reports.Items.Select(i => i.Label).ToArray();

        Assert.Equal(
            [
                "Sessions per Month per Stack",
                "Average Score per Month per Stack",
                "Back",
            ],
            labels
        );
    }

    private static IMenuItem BuildMenuFromDefaultProviders()
    {
        var stackRepository = new FakeStackRepository();
        var flashcardRepository = new FakeFlashcardRepository();
        var stackService = new StackService(stackRepository);
        var flashcardService = new FlashcardService(flashcardRepository);
        var studySessionService = new FakeStudySessionService();
        var reportService = new FakeReportService();
        var ui = new FakeAppUi();
        var menuPrompt = new FakeMenuPrompt();

        var providers = new IMainMenuSectionProvider[]
        {
            new ManageStacksSectionProvider(
                flashcardRepository,
                stackRepository,
                stackService,
                flashcardService,
                ui,
                menuPrompt
            ),
            new ManageFlashcardsSectionProvider(
                stackRepository,
                flashcardRepository,
                flashcardService,
                ui,
                menuPrompt
            ),
            new StudySessionsSectionProvider(stackRepository, studySessionService, ui, menuPrompt),
            new ReportsSectionProvider(reportService, ui, menuPrompt),
            new ExitSectionProvider(),
        };

        var composer = new MenuComposer(providers, menuPrompt);
        return composer.Build();
    }

    private sealed class FakeStudySessionService : IStudySessionService
    {
        public Task RunAsync(Stack stack) => Task.CompletedTask;

        public Task<IReadOnlyList<StudySession>> GetHistoryAsync()
        {
            return Task.FromResult<IReadOnlyList<StudySession>>([]);
        }
    }

    private sealed class FakeReportService : IReportService
    {
        public Task<IReadOnlyList<StackReportRow>> GetSessionsPerMonthPerStackAsync(int year)
        {
            return Task.FromResult<IReadOnlyList<StackReportRow>>([]);
        }

        public Task<IReadOnlyList<StackReportRow>> GetAverageScorePerMonthPerStackAsync(int year)
        {
            return Task.FromResult<IReadOnlyList<StackReportRow>>([]);
        }
    }

    private sealed class FakeMainMenuSectionProvider(string label, int order)
        : IMainMenuSectionProvider
    {
        public int Order => order;

        public IMenuItem Create() => new FakeMenuItem(label);
    }

    private sealed class FakeMenuItem(string label) : IMenuItem
    {
        public string Label => label;

        public Task<NavigationResult> ExecuteAsync() => Task.FromResult(NavigationResult.Continue);
    }

    private sealed class FakeAppUi : IAppUi
    {
        public void Clear() { }

        public void WriteSuccess(string message) { }

        public void WriteWarning(string message) { }

        public void WriteError(string message) { }

        public void WaitForKey(string message = "[grey]Press any key to continue...[/]") { }

        public string PromptText(string prompt, bool allowEmpty = false) => string.Empty;

        public bool Confirm(string prompt) => false;

        public Stack PromptStackSelection(string title, IReadOnlyList<Stack> stacks) =>
            stacks.FirstOrDefault() ?? new Stack { Name = string.Empty };

        public void ShowFlashcardsTable(string title, IEnumerable<FlashCardDto> cards) { }

        public void ShowStudySessionsTable(string title, IEnumerable<StudySession> sessions) { }

        public void ShowStackReportTable(string title, IEnumerable<StackReportRow> rows) { }
    }

    private sealed class FakeMenuPrompt : IMenuPrompt
    {
        public void Clear() { }

        public Task<IMenuItem?> PromptAsync(string title, IReadOnlyList<IMenuItem> items) =>
            Task.FromResult<IMenuItem?>(null);
    }

    private sealed class FakeStackRepository : IStackRepository
    {
        public Task<IEnumerable<Stack>> GetAllAsync() => Task.FromResult(Enumerable.Empty<Stack>());

        public Task<Stack> AddAsync(Stack stack) => Task.FromResult(stack);

        public Task DeleteAsync(int id) => Task.CompletedTask;
    }

    private sealed class FakeFlashcardRepository : IFlashcardRepository
    {
        public Task<IEnumerable<Flashcard>> GetByStackIdAsync(int stackId) =>
            Task.FromResult(Enumerable.Empty<Flashcard>());

        public Task AddAsync(Flashcard card) => Task.CompletedTask;

        public Task DeleteAsync(int id) => Task.CompletedTask;
    }
}
