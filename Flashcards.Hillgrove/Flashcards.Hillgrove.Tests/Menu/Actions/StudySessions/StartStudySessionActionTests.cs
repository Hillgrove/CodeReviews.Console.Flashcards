using Flashcards.Hillgrove.Menu.Actions.StudySessions;
using Flashcards.Hillgrove.Models;
using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Tests.Menu.Actions.StudySessions;

public class StartStudySessionActionTests
{
    [Fact]
    public async Task ExecuteAsync_DelegatesToStudySessionService_WithSelectedStack()
    {
        var service = new FakeStudySessionService();
        var action = new StartStudySessionAction(service);
        var stack = new Stack { Id = 12, Name = "Math" };

        await action.ExecuteAsync(stack);

        Assert.Same(stack, service.RunStack);
        Assert.Equal(1, service.RunCalls);
    }

    private sealed class FakeStudySessionService : IStudySessionService
    {
        public int RunCalls { get; private set; }
        public Stack? RunStack { get; private set; }

        public Task RunAsync(Stack stack)
        {
            RunCalls++;
            RunStack = stack;
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<StudySession>> GetHistoryAsync()
        {
            return Task.FromResult<IReadOnlyList<StudySession>>([]);
        }
    }
}
