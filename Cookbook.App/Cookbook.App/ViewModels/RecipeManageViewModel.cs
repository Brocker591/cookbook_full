using Cookbook.App.Models;
using Cookbook.App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.App.ViewModels
{
    [QueryProperty(nameof(Recipe), "Recipe")]
    public class RecipeManageViewModel : BaseViewModel
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

        public ObservableCollection<Ingredient> IngredientList { get; } = new();
        public Command SaveRecipeCommand { get; set; }
        public Command AddIngriedientCommand { get; set; }
        public Command RemoveIngriedientCommand { get; set; }
        public Command GoPageBackCommand { get; set; }


        public RecipeManageViewModel(ICookBookService cookBookService)
        {
            _cookBookService = cookBookService;
            SaveRecipeCommand = new Command(async () => await SaveRecipeAsync());
            AddIngriedientCommand = new Command(async () => await AddIngriedientAsync());
            RemoveIngriedientCommand = new Command(async (ingredient) => await RemoveIngriedientAsync((Ingredient)ingredient));
            GoPageBackCommand = new Command(async () => await GoPageBack());

            if (Recipe != null && Recipe.Ingredients != null)
            {
                foreach(var ingredient in Recipe.Ingredients)
                {
                    IngredientList.Add(ingredient);
                }
            }

        }

        private async Task AddIngriedientAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                Ingredient newIngredient = new Ingredient();
                IngredientList.Add(newIngredient);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RemoveIngriedientAsync(Ingredient ingredient)
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                IngredientList.Remove(ingredient);
                IsBusy = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }

        }



        private async Task SaveRecipeAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if(Recipe.Id == 0)
                {
                    Recipe.Ingredients = IngredientList.ToList();
                    await _cookBookService.CreateRecipeAsync(Recipe);
                    IsBusy = false;
                    await Shell.Current.Navigation.PopAsync();

                }
                else
                {
                    //Update Implementieren
                }
                    
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }




    }
}
