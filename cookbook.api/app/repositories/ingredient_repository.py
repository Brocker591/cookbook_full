from sqlalchemy.orm import Session
from fastapi import HTTPException, status
from typing import List
from app.schemas import IngredientBaseDto, IngredientDto
from app.repositories import datamodels

async def create_ingredient(ingredient_base: IngredientBaseDto, db: Session) -> IngredientDto:
    exist_recipe = db.query(datamodels.Recipe).filter(
        datamodels.Recipe.id == ingredient_base.recipe_id).first()

    if not exist_recipe:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND, detail="recipe not found")

    exist_ingredient_name = db.query(datamodels.Recipe).filter(
        datamodels.Ingredient.name == ingredient_base.name and datamodels.Recipe.id == ingredient_base.recipe_id).first()

    if exist_ingredient_name:
        raise HTTPException(
            status_code=status.HTTP_409_CONFLICT, detail="ingredients exist")

    new_ingredient = datamodels.Ingredient(
        name=ingredient_base.name, quantity=ingredient_base.quantity, recipe_id=ingredient_base.recipe_id)
    db.add(new_ingredient)
    db.commit()
    db.refresh(new_ingredient)
    created_ingredient = IngredientDto(id=new_ingredient.id, name=new_ingredient.name,
                                       quantity=new_ingredient.quantity, recipe_id=new_ingredient.recipe_id)

    return created_ingredient


async def get_all_ingredient(db: Session) -> List[IngredientDto]:
    all_ingredient = db.query(datamodels.Ingredient).all()

    all_ingredient_dto = [IngredientDto(
        id=x.id, name=x.name, quantity=x.quantity, recipe_id=x.recipe_id) for x in all_ingredient]
    return all_ingredient_dto


async def get_ingredient(ingredient_id: int, db: Session) -> IngredientDto:
    exist_ingredient = await get_ingredient_datamodel(ingredient_id=ingredient_id, db=db)

    ingredient_dto = IngredientDto(id=exist_ingredient.id, name=exist_ingredient.name,
                                   quantity=exist_ingredient.quantity, recipe_id=exist_ingredient.recipe_id)
    return ingredient_dto


async def get_ingredient_by_recipe_id(recipe_id: int, db: Session) -> List[IngredientDto]:
    ingredients = db.query(datamodels.Ingredient).filter(
        datamodels.Ingredient.recipe_id == recipe_id).first()

    all_ingredient_dto = [IngredientDto(
        id=x.id, name=x.name, quantity=x.quantity, recipe_id=x.recipe_id) for x in ingredients]
    return all_ingredient_dto


async def update_ingredient(ingredient: IngredientDto, db: Session) -> None:
    exist_ingredient = await get_ingredient_datamodel(ingredient_id=ingredient.id, db=db)

    exist_ingredient.name = ingredient.name
    exist_ingredient.quantity = ingredient.quantity
    db.add(exist_ingredient)
    db.commit()


async def delete_ingredient(ingredient_id: int, db: Session) -> None:
    ingredient_datamodel = await get_ingredient_datamodel(ingredient_id=ingredient_id, db=db)

    db.delete(ingredient_datamodel)
    db.commit()


async def check_if_ingredient_exist(ingredient_id: int, db: Session) -> bool:
    exist_ingredient = db.query(datamodels.Ingredient).filter(
        datamodels.Ingredient.id == ingredient_id).first()

    if not exist_ingredient:
        return False
    else:
        return True


async def get_ingredient_datamodel(ingredient_id: int, db: Session) -> datamodels.Ingredient:
    exist_ingredient = db.query(datamodels.Ingredient).filter(
        datamodels.Ingredient.id == ingredient_id).first()

    if not exist_ingredient:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND, detail="Ingredient not found")
    return exist_ingredient
