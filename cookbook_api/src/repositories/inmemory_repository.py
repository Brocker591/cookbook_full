from fastapi import status, HTTPException
from src import schemas
from typing import Optional, List

list_of_recipes = []


async def create_recipe(recipe_create_Dto: schemas.RecipeCreateDto) -> schemas.RecipeDto:
    id = len(list_of_recipes) + 1
    list_of_incredients = []

    for ingredient in recipe_create_Dto.ingredients:
        newIngredient = schemas.IngredientDto(name=ingredient.name, quantity=ingredient.quantity,recipe_id=id)
        list_of_incredients.append(newIngredient)


    new_recipe = schemas.RecipeDto(id=id, description=recipe_create_Dto.description, preparation=recipe_create_Dto.preparation, ingredients=list_of_incredients)
    list_of_recipes.append(new_recipe)

    return new_recipe


async def get_all_recipe() -> List[schemas.RecipeDto]:
    return list_of_recipes


async def get_recipe(id: int) -> schemas.RecipeDto:

    find_recipe = None

    for recipe in list_of_recipes:
        if recipe.id == id:
            find_recipe = recipe

    if find_recipe != None:
        return find_recipe
    else:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="recipe not found")
        
async def update_recipe(recipe: schemas.RecipeDto) -> None:

    recipe_to_update = await get_recipe(recipe.id)

    recipe_to_update.description = recipe.description
    recipe_to_update.preparation = recipe.preparation
    recipe_to_update.ingredients = recipe.ingredients


async def delete_recipe(id: int) -> None:
    recipe_to_delete = await get_recipe(id)

    list_of_recipes.remove(recipe_to_delete)


