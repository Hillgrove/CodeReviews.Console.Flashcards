using Flashcards.Hillgrove.Helpers;
using Flashcards.Hillgrove.Menu.Actions.Stacks;
using Flashcards.Hillgrove.Models;
using Flashcards.Hillgrove.Services;

namespace Flashcards.Hillgrove.Menu.Items.Stacks
{
    internal class CreateStackMenuItem : IMenuItem
    {
        private readonly IStackService _service;
        private readonly IStackAction _onCreated;
        private readonly IAppUi _ui;

        public string Label => "Create a new stack";

        public CreateStackMenuItem(IStackService service, IStackAction onCreated, IAppUi ui)
        {
            _service = service;
            _onCreated = onCreated;
            _ui = ui;
        }

        public async Task<NavigationResult> ExecuteAsync()
        {
            string input;

            while (true)
            {
                _ui.Clear();
                _ui.WriteSuccess("Create a new stack\n");

                input = _ui.PromptText(
                    "What should the stack be called? [grey](leave blank to cancel)[/]",
                    allowEmpty: true
                );

                if (string.IsNullOrWhiteSpace(input))
                {
                    return NavigationResult.Continue;
                }

                if (!_ui.Confirm($"Use this name: '{input}'? "))
                {
                    _ui.Clear();
                    continue;
                }

                Stack newStack = new Stack { Name = input };
                Stack? stackAdded = await _service.AddAsync(newStack);

                if (stackAdded == null)
                {
                    _ui.WriteError($"\nName already exists: '{input}'");

                    if (_ui.Confirm("\nTry again with another name?"))
                    {
                        continue;
                    }
                }
                else
                {
                    _ui.WriteSuccess($"\nStack added: '{input}'");

                    if (_ui.Confirm("\nAdd flashcards to the new stack now?"))
                    {
                        await _onCreated.ExecuteAsync(stackAdded);
                    }
                }

                return NavigationResult.Continue;
            }
        }
    }
}
