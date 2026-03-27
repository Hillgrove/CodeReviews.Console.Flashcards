using Flashcards.Hillgrove.Dtos;
using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Menu
{
    internal interface IAppUi
    {
        void Clear();
        void WriteSuccess(string message);
        void WriteWarning(string message);
        void WriteError(string message);
        void WaitForKey(string message = "[grey]Press any key to continue...[/]");
        string PromptText(string prompt, bool allowEmpty = false);
        bool Confirm(string prompt);
        Stack PromptStackSelection(string title, IReadOnlyList<Stack> stacks);
        void ShowFlashcardsTable(string title, IEnumerable<FlashCardDto> cards);
        void ShowStudySessionsTable(string title, IEnumerable<StudySession> sessions);
        void ShowStackReportTable(string title, IEnumerable<StackReportRow> rows);
    }
}
