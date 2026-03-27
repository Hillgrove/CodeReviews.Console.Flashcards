using Flashcards.Hillgrove.Menu.Items;

namespace Flashcards.Hillgrove.Menu.Composition
{
    internal class ExitSectionProvider : IMainMenuSectionProvider
    {
        public int Order => 5;

        public IMenuItem Create() => new ExitMenuItem();
    }
}
