using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Data
{
    internal interface IStudySessionRepository
    {
        Task AddAsync(StudySession studySession);
        Task<IEnumerable<StudySession>> GetAllAsync();
    }
}
