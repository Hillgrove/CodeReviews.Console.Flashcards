using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Helpers;
using Flashcards.Hillgrove.Menu.Actions.Stacks;

namespace Flashcards.Hillgrove.Menu.Items.Stacks
{
    internal class ShowAllStacksMenuItem : IMenuItem
    {
        private readonly IStackRepository _repository;
        private readonly IStackAction _onSelect;
        private readonly IAppUi _ui;
        private readonly string _label;
        private readonly string _promptTitle;

        public string Label => _label;

        public ShowAllStacksMenuItem(
            IStackRepository repository,
            IStackAction onSelect,
            IAppUi ui,
            string label = "Show All Stacks",
            string promptTitle = "[green]Choose a stack[/]"
        )
        {
            _repository = repository;
            _onSelect = onSelect;
            _ui = ui;
            _label = label;
            _promptTitle = promptTitle;
        }

        public async Task<NavigationResult> ExecuteAsync()
        {
            var stacks = (await _repository.GetAllAsync()).ToList();

            if (stacks.Count == 0)
            {
                _ui.WriteWarning("There are no stacks available.");
                _ui.WaitForKey();
                return NavigationResult.Continue;
            }

            var selected = _ui.PromptStackSelection(_promptTitle, stacks);
            await _onSelect.ExecuteAsync(selected);

            return NavigationResult.Continue;
        }
    }
}
