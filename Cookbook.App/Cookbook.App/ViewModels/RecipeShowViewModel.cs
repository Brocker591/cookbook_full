using Cookbook.App.Models;
using Cookbook.App.Services;
using Cookbook.App.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.App.ViewModels
{
    [QueryProperty(nameof(Models.Recipe), "Recipe")]
    public class RecipeShowViewModel : BaseViewModel
    {
        private readonly ICookBookService _cookBookService;
        private Recipe recipe;
        public Recipe Recipe
        {
            get => recipe;
            set
            {
                if (recipe == value)
                    return;

                recipe = value;
                OnPropertyChanged();
            }
        }


        public Command GoToRecipeManageCommand { get; set; }
        public Command GoPageBackCommand { get; set; }


        public RecipeShowViewModel(ICookBookService cookBookService)
        {
            _cookBookService = cookBookService;
            GoToRecipeManageCommand = new Command(async () => await GoToRecipeManageAsync());
            GoPageBackCommand = new Command(async () => await GoPageBack());
        }

        public async Task GoToRecipeManageAsync()
        {
            await Shell.Current.GoToAsync(nameof(RecipeManagePage), true, new Dictionary<string, object>
            {
                {nameof(Models.Recipe), Recipe }
            });
        }
    }
}
