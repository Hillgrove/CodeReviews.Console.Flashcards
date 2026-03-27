using Flashcards.Hillgrove.Menu.Composition;

namespace Flashcards.Hillgrove.Menu
{
    internal class MenuComposer
    {
        private readonly IEnumerable<IMainMenuSectionProvider> _sectionProviders;
        private readonly IMenuPrompt _menuPrompt;

        public MenuComposer(
            IEnumerable<IMainMenuSectionProvider> sectionProviders,
            IMenuPrompt menuPrompt
        )
        {
            _sectionProviders = sectionProviders;
            _menuPrompt = menuPrompt;
        }

        public IMenuItem Build()
        {
            var sections = _sectionProviders
                .OrderBy(provider => provider.Order)
                .Select(provider => provider.Create());

            return new CompositeMenu("Main Menu", sections, _menuPrompt);
        }
    }
}
