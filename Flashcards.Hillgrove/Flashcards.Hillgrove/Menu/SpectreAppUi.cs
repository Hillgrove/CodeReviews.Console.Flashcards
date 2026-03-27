using Flashcards.Hillgrove.Dtos;
using Flashcards.Hillgrove.Models;
using Spectre.Console;

namespace Flashcards.Hillgrove.Menu
{
    internal class SpectreAppUi : IAppUi
    {
        public void Clear() => AnsiConsole.Clear();

        public void WriteSuccess(string message) => AnsiConsole.MarkupLine($"[green]{message}[/]");

        public void WriteWarning(string message) => AnsiConsole.MarkupLine($"[yellow]{message}[/]");

        public void WriteError(string message) => AnsiConsole.MarkupLine($"[red]{message}[/]");

        public void WaitForKey(string message = "[grey]Press any key to continue...[/]")
        {
            AnsiConsole.Markup($"\n{message}");
            Console.ReadKey(true);
        }

        public string PromptText(string prompt, bool allowEmpty = false)
        {
            var textPrompt = new TextPrompt<string>(prompt);

            if (allowEmpty)
            {
                textPrompt.AllowEmpty();
            }

            return AnsiConsole.Prompt(textPrompt);
        }

        public bool Confirm(string prompt) => AnsiConsole.Confirm(prompt);

        public Stack PromptStackSelection(string title, IReadOnlyList<Stack> stacks)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<Stack>()
                    .Title(title)
                    .UseConverter(stack => stack.Name)
                    .AddChoices(stacks)
            );
        }

        public void ShowFlashcardsTable(string title, IEnumerable<FlashCardDto> cards)
        {
            var table = new Table();
            table.AddColumn("#");
            table.AddColumn("Question");
            table.AddColumn("Answer");

            foreach (var card in cards)
            {
                table.AddRow(
                    card.DisplayIndex.ToString(),
                    card.Question ?? string.Empty,
                    card.Answer ?? string.Empty
                );
            }

            WriteSuccess(title);
            AnsiConsole.Write(table);
        }

        public void ShowStudySessionsTable(string title, IEnumerable<StudySession> sessions)
        {
            var table = new Table();
            table.AddColumn("Date (UTC)");
            table.AddColumn("Stack");
            table.AddColumn("Score");

            foreach (var session in sessions)
            {
                table.AddRow(
                    session.Date.ToString("u"),
                    session.StackName ?? session.StackId.ToString(),
                    session.Score.ToString()
                );
            }

            WriteSuccess(title);
            AnsiConsole.Write(table);
        }

        public void ShowStackReportTable(string title, IEnumerable<StackReportRow> rows)
        {
            var table = new Table();
            table.AddColumn("Stack");
            table.AddColumn("Jan");
            table.AddColumn("Feb");
            table.AddColumn("Mar");
            table.AddColumn("Apr");
            table.AddColumn("May");
            table.AddColumn("Jun");
            table.AddColumn("Jul");
            table.AddColumn("Aug");
            table.AddColumn("Sep");
            table.AddColumn("Oct");
            table.AddColumn("Nov");
            table.AddColumn("Dec");
            table.AddColumn("Total");
            table.AddColumn("Average");

            foreach (var row in rows)
            {
                table.AddRow(
                    row.StackName,
                    FormatReportValue(row.January),
                    FormatReportValue(row.February),
                    FormatReportValue(row.March),
                    FormatReportValue(row.April),
                    FormatReportValue(row.May),
                    FormatReportValue(row.June),
                    FormatReportValue(row.July),
                    FormatReportValue(row.August),
                    FormatReportValue(row.September),
                    FormatReportValue(row.October),
                    FormatReportValue(row.November),
                    FormatReportValue(row.December),
                    FormatReportValue(row.Total),
                    FormatReportValue(row.Average)
                );
            }

            WriteSuccess(title);
            AnsiConsole.Write(table);
        }

        private static string FormatReportValue(double value)
        {
            var roundedValue = Math.Round(value, 2);

            if (Math.Abs(roundedValue % 1) < 0.000001)
            {
                return roundedValue.ToString("0");
            }

            return roundedValue.ToString("0.##");
        }
    }
}
