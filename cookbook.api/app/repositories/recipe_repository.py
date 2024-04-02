from fastapi import HTTPException, status
from sqlalchemy.orm import Session
from typing import List
from app.repositories import datamodels, ingredient_repository
from app.schemas import RecipeDto, RecipeCreateDto, IngredientDto

async def create_recipe(recipe: RecipeCreateDto, db: Session) -> RecipeDto:
    exist_reicpe_name = db.query(datamodels.Recipe).filter(
        datamodels.Recipe.name == recipe.name).first()

    if exist_reicpe_name:
        raise HTTPException(
            status_code=status.HTTP_409_CONFLICT, detail="ingredients exist")

    list_ingredient = [datamodels.Ingredient(
        name=x.name, quantity=x.quantity) for x in recipe.ingredients]

    db_recipe = datamodels.Recipe(name=recipe.name,
                                  preparation=recipe.preparation, ingredients=list_ingredient)
    db.add(db_recipe)
    db.commit()
    db.refresh(db_recipe)

    # for ingredient in recipe.ingredients:
    #     ingredient.recipe_id = db_recipe.id

    #     ingredient_repository.create_ingredient(
    #         ingredient_base=ingredient, db=db, recipe_id=db_recipe.id)

    created_recipe = RecipeDto(**db_recipe.model_dump())
    return created_recipe


async def get_all_recipe(db: Session) -> List[RecipeDto]:
    all_recipe = db.query(datamodels.Recipe).all()

    all_recipe_dto = []

    for recipe in all_recipe:
        ingredient_dto = [IngredientDto(
            id=x.id, name=x.name, quantity=x.quantity, recipe_id=x.recipe_id) for x in recipe.ingredients]

        recipe_dto = RecipeDto(id=recipe.id, name=recipe.name,
                               preparation=recipe.preparation,  ingredients=ingredient_dto)
        all_recipe_dto.append(recipe_dto)

    return all_recipe_dto


async def get_recipe(recipe_id: int, db: Session) -> RecipeDto:
    recipe = await get_recipe_datamodel(recipe_id=recipe_id, db=db)

    ingredient_dto = [IngredientDto(
        id=x.id, name=x.name, quantity=x.quantity, recipe_id=x.recipe_id) for x in recipe.ingredients]

    recipe_dto = RecipeDto(id=recipe.id, name=recipe.name,
                           preparation=recipe.preparation,  ingredients=ingredient_dto)
    return recipe_dto


async def update_recipe(recipe: RecipeDto, db: Session) -> None:
    exist_recipe = await get_recipe_datamodel(recipe_id=recipe.id, db=db)

    exist_recipe.name = recipe.name
    exist_recipe.preparation = recipe.preparation

    for ingredient in recipe.ingredients:

        exist_ingredient = await ingredient_repository.check_if_ingredient_exist(ingredient_id=ingredient.id, db=db)

        if exist_ingredient:
            await ingredient_repository.update_ingredient(ingredient=ingredient, db=db)
        else:
            await ingredient_repository.create_ingredient(ingredient_base=ingredient, db=db)

    db.add(exist_recipe)
    db.commit()


async def delete_recipe(recipe_id: int, db: Session) -> None:
    exist_recipe = await get_recipe_datamodel(recipe_id=recipe_id, db=db)

    for ingredient in exist_recipe.ingredients:
        await ingredient_repository.delete_ingredient(ingredient_id=ingredient.id, db=db)

    db.delete(exist_recipe)
    db.commit()


async def get_recipe_datamodel(recipe_id: int, db: Session) -> datamodels.Recipe:
    exist_recipe = db.query(datamodels.Recipe).filter(
        datamodels.Recipe.id == recipe_id).first()

    if not exist_recipe:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND, detail="recipe not found")

    return exist_recipe
