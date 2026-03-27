namespace Flashcards.Hillgrove.Menu.Composition
{
    internal interface IMainMenuSectionProvider
    {
        int Order { get; }
        IMenuItem Create();
    }
}
