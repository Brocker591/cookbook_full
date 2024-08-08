using Cookbook.App.Views;

namespace Cookbook.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(AddOrCreateItemPage), typeof(AddOrCreateItemPage));
        Routing.RegisterRoute(nameof(RecipeShowPage), typeof(RecipeShowPage));
        Routing.RegisterRoute(nameof(RecipeManagePage), typeof(RecipeManagePage));
        Routing.RegisterRoute(nameof(EditMealPlanPage), typeof(EditMealPlanPage));

    }
}
