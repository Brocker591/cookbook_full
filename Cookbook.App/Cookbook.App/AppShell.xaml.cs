using Cookbook.App.Views;

namespace Cookbook.App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddOrCreateItemPage), typeof(AddOrCreateItemPage));
        }
    }
}
