using Flashcards.Hillgrove.Helpers;
using Flashcards.Hillgrove.Menu.Items;

namespace Flashcards.Hillgrove.Tests.Menu;

public class MenuItemsTests
{
    [Fact]
    public async Task ExecuteAsync_ReturnsBack_WhenMenuItemIsBack()
    {
        var item = new BackMenuItem();

        var result = await item.ExecuteAsync();

        Assert.Equal(NavigationResult.Back, result);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsExit_WhenMenuItemIsExit()
    {
        var item = new ExitMenuItem();

        var result = await item.ExecuteAsync();

        Assert.Equal(NavigationResult.Exit, result);
    }
}
