using System.Data;
using Flashcards.Hillgrove.Data;
using Flashcards.Hillgrove.Menu;
using Flashcards.Hillgrove.Menu.Composition;
using Flashcards.Hillgrove.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

string connectionString =
    config.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string is missing.");

var services = new ServiceCollection();
services.AddSingleton<IDbConnection>(_ => new SqlConnection(connectionString));

services.AddSingleton<IStackRepository, StackRepository>();
services.AddSingleton<IFlashcardRepository, FlashcardRepository>();
services.AddSingleton<IStudySessionRepository, StudySessionRepository>();
services.AddSingleton<IReportRepository, ReportRepository>();

services.AddSingleton<IStackService, StackService>();
services.AddSingleton<IFlashcardService, FlashcardService>();
services.AddSingleton<IStudySessionService, StudySessionService>();
services.AddSingleton<IReportService, ReportService>();

services.AddSingleton<IAppUi, SpectreAppUi>();
services.AddSingleton<IMenuPrompt, SpectreMenuPrompt>();

services.AddSingleton<IMainMenuSectionProvider, ManageStacksSectionProvider>();
services.AddSingleton<IMainMenuSectionProvider, ManageFlashcardsSectionProvider>();
services.AddSingleton<IMainMenuSectionProvider, StudySessionsSectionProvider>();
services.AddSingleton<IMainMenuSectionProvider, ReportsSectionProvider>();
services.AddSingleton<IMainMenuSectionProvider, ExitSectionProvider>();

services.AddSingleton<MenuComposer>();

using var provider = services.BuildServiceProvider();

var menuComposer = provider.GetRequiredService<MenuComposer>();
var menu = menuComposer.Build();
await menu.ExecuteAsync();
