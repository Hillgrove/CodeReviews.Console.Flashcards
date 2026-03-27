using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Services
{
    internal interface IStudySessionService
    {
        Task RunAsync(Stack stack);
        Task<IReadOnlyList<StudySession>> GetHistoryAsync();
    }
}
