using Flashcards.Hillgrove.Helpers;

namespace Flashcards.Hillgrove.Menu.Items
{
    internal class ExitMenuItem : IMenuItem
    {
        public string Label => "Exit";

        public Task<NavigationResult> ExecuteAsync() => Task.FromResult(NavigationResult.Exit);
    }
}
