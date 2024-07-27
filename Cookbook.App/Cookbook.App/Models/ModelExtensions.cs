using Cookbook.App.Dtos;


namespace Cookbook.App.Models;

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

    public static Ingredient ToModel(this IngredientUpdateDto ingredientUpdateDto)
    {
        return new Ingredient
        {
            Id = ingredientUpdateDto.id,
            Name = ingredientUpdateDto.name,
            Quantity = ingredientUpdateDto.quantity,
            RecipeId = ingredientUpdateDto.recipe_id
        };
    }

    public static IngredientUpdateDto ToUpdateDto(this Ingredient ingredient)
    {
        return new IngredientUpdateDto
        {
            id = ingredient.Id,
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

    public static Recipe ToModel(this RecipeUpdateDto recipeUpdateDto)
    {
        return new Recipe
        {
            Id = recipeUpdateDto.id,
            Name = recipeUpdateDto.name,
            Preparation = recipeUpdateDto.preparation,
            Ingredients = recipeUpdateDto.ingredients.Select(i => i.ToModel()).ToList()
        };
    }

    public static RecipeUpdateDto ToUpdateDto(this Recipe recipe)
    {
        return new RecipeUpdateDto
        {
            id = recipe.Id,
            name = recipe.Name,
            preparation = recipe.Preparation,
            ingredients = recipe.Ingredients.Select(i => i.ToUpdateDto()).ToList()
        };
    }


}
