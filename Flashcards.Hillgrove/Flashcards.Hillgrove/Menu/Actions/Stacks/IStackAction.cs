using Flashcards.Hillgrove.Models;

namespace Flashcards.Hillgrove.Menu.Actions.Stacks
{
    internal interface IStackAction
    {
        Task ExecuteAsync(Stack stack);
    }
}
