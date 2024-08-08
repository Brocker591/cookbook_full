using Cookbook.App.Repositories;
using Cookbook.App.Services;
using Cookbook.App.ViewModels;
using Cookbook.App.Views;
using Microsoft.Extensions.Logging;

namespace Cookbook.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        DatabaseSetting databaseSetting = new(FileSystem.AppDataDirectory);

        //Repositories
        builder.Services.AddSingleton(databaseSetting);
        builder.Services.AddSingleton<IUserRepository, UserRepository>();
        builder.Services.AddSingleton<IItemRepository, ItemRepository>();
        builder.Services.AddSingleton<IItemToSendRepository, ItemToSendRepository>();


        //Services
        builder.Services.AddSingleton<ICookBookService, CookBookService>();
        
        //ViewModels
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<ShoppingListViewModel>();
        builder.Services.AddSingleton<AddOrCreateItemViewModel>();
        builder.Services.AddSingleton<RecipeListViewModel>();
        builder.Services.AddSingleton<RecipeManageViewModel>();
        builder.Services.AddSingleton<RecipeShowViewModel>();
        builder.Services.AddSingleton<MealPlanViewModel>();
        builder.Services.AddSingleton<EditMealPlanViewModel>();

        //Views
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<ShoppingListPage>();
        builder.Services.AddSingleton<AddOrCreateItemPage>();
        builder.Services.AddSingleton<RecipeListPage>();
        builder.Services.AddSingleton<RecipeManagePage>();
        builder.Services.AddSingleton<RecipeShowPage>();
        builder.Services.AddSingleton<MealPlanPage>();
        builder.Services.AddSingleton<EditMealPlanPage>();




#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
