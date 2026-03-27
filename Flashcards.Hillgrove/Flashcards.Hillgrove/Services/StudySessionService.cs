using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Menu;
using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Services
{
    internal class StudySessionService : IStudySessionService
    {
        private readonly IFlashcardRepository _flashcardRepository;
        private readonly IStudySessionRepository _studySessionRepository;
        private readonly IAppUi _ui;

        public StudySessionService(
            IFlashcardRepository flashcardRepository,
            IStudySessionRepository studySessionRepository,
            IAppUi ui
        )
        {
            _flashcardRepository = flashcardRepository;
            _studySessionRepository = studySessionRepository;
            _ui = ui;
        }

        public async Task RunAsync(Stack stack)
        {
            var flashcards = (await _flashcardRepository.GetByStackIdAsync(stack.Id)).ToList();

            if (flashcards.Count == 0)
            {
                _ui.WriteWarning($"There are no flashcards in '{stack.Name}'.");
                _ui.WaitForKey();
                return;
            }

            _ui.Clear();
            _ui.WriteSuccess($"Study session for '{stack.Name}'\n");

            var score = 0;

            for (var index = 0; index < flashcards.Count; index++)
            {
                var card = flashcards[index];
                _ui.WriteSuccess($"Question {index + 1} of {flashcards.Count}");
                _ui.WriteSuccess(card.Question ?? string.Empty);

                var userAnswer = _ui.PromptText("Your answer:").Trim();
                var correctAnswer = (card.Answer ?? string.Empty).Trim();

                if (string.Equals(userAnswer, correctAnswer, StringComparison.OrdinalIgnoreCase))
                {
                    score++;
                    _ui.WriteSuccess("Correct!\n");
                }
                else
                {
                    _ui.WriteWarning($"Incorrect. Correct answer: {card.Answer}\n");
                }
            }

            await _studySessionRepository.AddAsync(
                new StudySession
                {
                    StackId = stack.Id,
                    Date = DateTime.UtcNow,
                    Score = score,
                }
            );

            _ui.WriteSuccess($"Session complete. Score: {score}/{flashcards.Count}");
            _ui.WaitForKey();
        }

        public async Task<IReadOnlyList<StudySession>> GetHistoryAsync()
        {
            var sessions = await _studySessionRepository.GetAllAsync();
            return sessions.ToList();
        }
    }
}
