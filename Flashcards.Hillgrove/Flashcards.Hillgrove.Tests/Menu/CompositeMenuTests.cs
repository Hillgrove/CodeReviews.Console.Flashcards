using Flashcards.Hillgrove.Helpers;
using Flashcards.Hillgrove.Menu;

namespace Flashcards.Hillgrove.Tests.Menu;

public class CompositeMenuTests
{
    [Fact]
    public async Task ExecuteAsync_ReturnsContinue_WhenSelectedItemReturnsBack()
    {
        var prompt = new FakeMenuPrompt([new FakeMenuItem("Back", NavigationResult.Back)]);

        var sut = new CompositeMenu(
            "Any Menu",
            [new FakeMenuItem("Ignored", NavigationResult.Continue)],
            prompt
        );

        var result = await sut.ExecuteAsync();

        Assert.Equal(NavigationResult.Continue, result);
        Assert.Equal(1, prompt.CallCount);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsExit_WhenSelectedItemReturnsExit()
    {
        var prompt = new FakeMenuPrompt([new FakeMenuItem("Exit", NavigationResult.Exit)]);

        var sut = new CompositeMenu(
            "Any Menu",
            [new FakeMenuItem("Ignored", NavigationResult.Continue)],
            prompt
        );

        var result = await sut.ExecuteAsync();

        Assert.Equal(NavigationResult.Exit, result);
        Assert.Equal(1, prompt.CallCount);
    }

    [Fact]
    public async Task ExecuteAsync_KeepsLoopingUntilExit_WhenSelectedItemsReturnContinueThenExit()
    {
        var prompt = new FakeMenuPrompt([
            new FakeMenuItem("Continue", NavigationResult.Continue),
            new FakeMenuItem("Exit", NavigationResult.Exit),
        ]);

        var sut = new CompositeMenu(
            "Any Menu",
            [new FakeMenuItem("Ignored", NavigationResult.Continue)],
            prompt
        );

        var result = await sut.ExecuteAsync();

        Assert.Equal(NavigationResult.Exit, result);
        Assert.Equal(2, prompt.CallCount);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsContinue_WhenPromptReturnsNullChoice()
    {
        var prompt = new FakeMenuPrompt([null]);

        var sut = new CompositeMenu(
            "Any Menu",
            [new FakeMenuItem("Ignored", NavigationResult.Continue)],
            prompt
        );

        var result = await sut.ExecuteAsync();

        Assert.Equal(NavigationResult.Continue, result);
        Assert.Equal(1, prompt.CallCount);
    }

    private sealed class FakeMenuPrompt : IMenuPrompt
    {
        private readonly Queue<IMenuItem?> _choices;

        public int CallCount { get; private set; }

        public FakeMenuPrompt(IEnumerable<IMenuItem?> choices)
        {
            _choices = new Queue<IMenuItem?>(choices);
        }

        public void Clear()
        {
            // do nothing
        }

        public Task<IMenuItem?> PromptAsync(string title, IReadOnlyList<IMenuItem> items)
        {
            CallCount++;

            if (_choices.Count == 0)
            {
                throw new InvalidOperationException("No more queued choices in FakeMenuPrompt.");
            }

            return Task.FromResult(_choices.Dequeue());
        }
    }

    private sealed class FakeMenuItem : IMenuItem
    {
        private readonly NavigationResult _result;

        public string Label { get; }

        public FakeMenuItem(string label, NavigationResult result)
        {
            Label = label;
            _result = result;
        }

        public Task<NavigationResult> ExecuteAsync() => Task.FromResult(_result);
    }
}
