namespace Flashcards.Hillgrove.Menu
{
    internal interface IMenuPrompt
    {
        void Clear();
        Task<IMenuItem?> PromptAsync(string title, IReadOnlyList<IMenuItem> items);
    }
}
