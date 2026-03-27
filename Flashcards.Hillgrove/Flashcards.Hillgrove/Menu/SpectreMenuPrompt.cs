using Spectre.Console;

namespace Flashcards.Hillgrove.Menu
{
    internal class SpectreMenuPrompt : IMenuPrompt
    {
        public void Clear() => AnsiConsole.Clear();

        public Task<IMenuItem?> PromptAsync(string label, IReadOnlyList<IMenuItem> items)
        {
            ArgumentNullException.ThrowIfNull(label);
            ArgumentNullException.ThrowIfNull(items);

            if (items.Count == 0)
            {
                AnsiConsole.MarkupLine($"[green]{label}[/]\n");
                AnsiConsole.MarkupLine("[yellow]No options available yet.[/]");
                AnsiConsole.Markup("\n[grey]Press any key to go back...[/]");
                Console.ReadKey(true);
                return Task.FromResult<IMenuItem?>(null);
            }

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<IMenuItem>()
                    .Title($"[green]{label}[/]")
                    .AddChoices(items)
                    .UseConverter(item => item.Label)
            );

            return Task.FromResult<IMenuItem?>(choice);
        }
    }
}
