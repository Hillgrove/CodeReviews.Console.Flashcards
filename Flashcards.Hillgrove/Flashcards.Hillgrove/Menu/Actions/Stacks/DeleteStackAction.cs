using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Menu.Actions.Stacks
{
    internal class DeleteStackAction : IStackAction
    {
        private readonly IStackRepository _repository;
        private readonly IAppUi _ui;

        public DeleteStackAction(IStackRepository repository, IAppUi ui)
        {
            _repository = repository;
            _ui = ui;
        }

        public async Task ExecuteAsync(Stack stack)
        {
            _ui.WriteSuccess($"Deleting '{stack.Name}'\n");

            if (!_ui.Confirm($"Delete stack '{stack.Name}'?"))
            {
                return;
            }

            if (
                !_ui.Confirm(
                    $"[red]This will permanently delete '{stack.Name}' and related records. Continue?[/]"
                )
            )
            {
                return;
            }

            await _repository.DeleteAsync(stack.Id);

            _ui.WriteSuccess($"Deleted stack: '{stack.Name}'");
            _ui.WaitForKey();
        }
    }
}
