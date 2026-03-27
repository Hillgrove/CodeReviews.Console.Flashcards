using Flashcards.Hillgrove.Helpers;

namespace Flashcards.Hillgrove.Menu
{
    internal interface IMenuItem
    {
        string Label { get; }
        Task<NavigationResult> ExecuteAsync();
    }
}
