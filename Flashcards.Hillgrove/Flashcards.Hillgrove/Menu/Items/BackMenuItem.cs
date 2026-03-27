using Flashcards.Hillgrove.Helpers;

namespace Flashcards.Hillgrove.Menu.Items
{
    internal class BackMenuItem : IMenuItem
    {
        public string Label => "Back";

        public Task<NavigationResult> ExecuteAsync() => Task.FromResult(NavigationResult.Back);
    }
}
