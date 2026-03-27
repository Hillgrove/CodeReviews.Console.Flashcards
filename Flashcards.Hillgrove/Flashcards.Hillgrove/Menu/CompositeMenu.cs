using Flashcards.Hillgrove.Helpers;

namespace Flashcards.Hillgrove.Menu
{
    internal class CompositeMenu : IMenuItem
    {
        private readonly IMenuPrompt _prompt;
        internal IReadOnlyList<IMenuItem> Items { get; }

        public string Label { get; }

        public CompositeMenu(string label, IEnumerable<IMenuItem> items, IMenuPrompt prompt)
        {
            _prompt = prompt;
            Items = items.ToList();
            Label = label;
        }

        public async Task<NavigationResult> ExecuteAsync()
        {
            while (true)
            {
                _prompt.Clear();

                var choice = await _prompt.PromptAsync(Label, Items);

                if (choice is null)
                {
                    return NavigationResult.Continue;
                }

                var result = await choice.ExecuteAsync();

                if (result == NavigationResult.Back)
                    return NavigationResult.Continue;

                if (result == NavigationResult.Exit)
                    return NavigationResult.Exit;
            }
        }
    }
}
