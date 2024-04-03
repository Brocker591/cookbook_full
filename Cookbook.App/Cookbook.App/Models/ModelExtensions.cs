using Cookbook.App.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.App.Models
{
    public static class ModelExtensions
    {
        public static ItemDto ToDto(this Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity,
                Priority = item.Priority,
                Inventory = item.Inventory
                
            };
        }

        public static Item ToModel(this ItemDto itemDto)
        {
            return new Item
            {
                Id = itemDto.Id,
                Name = itemDto.Name,
                Quantity = itemDto.Quantity,
                Priority = itemDto.Priority,
                Inventory = itemDto.Inventory
            };
        }

        public static ItemCreateDto ToCreateDto(this Item item)
        {
            return new ItemCreateDto
            {
                name = item.Name,
                quantity = item.Quantity
            };
        }

        public static IngredientDto ToDto(this Ingredient ingredient)
        {
            return new IngredientDto
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                quantity = ingredient.Quantity,
                RecipeId = ingredient.RecipeId
            };
        }

        public static Ingredient ToModel(this IngredientDto ingredientDto)
        {
            return new Ingredient
            {
                Id = ingredientDto.Id,
                Name = ingredientDto.Name,
                Quantity = ingredientDto.quantity,
                RecipeId = ingredientDto.RecipeId
            };
        }

        public static IngredientCreateDto ToCreateDto(this Ingredient ingredient)
        {
            return new IngredientCreateDto
            {
                name = ingredient.Name,
                quantity = ingredient.Quantity,
                recipe_id = ingredient.RecipeId
            };
        }

        public static RecipeDto ToDto(this Recipe recipe)
        {
            return new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Preparation = recipe.Preparation,
                Ingredients = recipe.Ingredients.Select(i => i.ToDto()).ToList()
            };
        }

        public static Recipe ToModel(this RecipeDto recipeDto)
        {
            return new Recipe
            {
                Id = recipeDto.Id,
                Name = recipeDto.Name,
                Preparation = recipeDto.Preparation,
                Ingredients = recipeDto.Ingredients.Select(i => i.ToModel()).ToList()
            };
        }

        public static RecipeCreateDto ToCreateDto(this Recipe recipe)
        {
            return new RecipeCreateDto
            {
                name = recipe.Name,
                preparation = recipe.Preparation,
                ingredients = recipe.Ingredients.Select(i => i.ToCreateDto()).ToList()
            };
        }


    }
}
