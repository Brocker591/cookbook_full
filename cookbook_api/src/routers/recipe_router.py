from fastapi import FastAPI, Response, status, HTTPException, Depends, APIRouter
from src.repositories import inmemory_repository as repository
from typing import Optional, List
from src import schemas
from src.auth import get_current_user
from src.repositories import datamodels


router = APIRouter(tags=["Recipes"])


@router.get("/recipes", status_code=status.HTTP_200_OK,  response_model=List[schemas.RecipeDto])
async def get_all_recipe(current_user: datamodels.User = Depends(get_current_user)) -> List[schemas.RecipeDto]:
    print(current_user)
    recipes = await repository.get_all_recipe()
    return recipes


@router.get("/recipes/{recipe_id}", status_code=status.HTTP_200_OK, response_model=schemas.RecipeDto)
async def get_recipe(recipe_id: int, current_user: datamodels.User = Depends(get_current_user)) -> schemas.RecipeDto:
    print(current_user)
    recipe = await repository.get_recipe(recipe_id)
    return recipe


@router.post("/recipes", status_code=status.HTTP_201_CREATED, response_model=schemas.RecipeDto)
async def create_recipe(recipe_create_Dto: schemas.RecipeCreateDto, current_user: datamodels.User = Depends(get_current_user)) -> schemas.RecipeDto:
    print(current_user)
    new_recipe = await repository.create_recipe(recipe_create_Dto=recipe_create_Dto)
    return new_recipe


@router.put("/recipes", status_code=status.HTTP_204_NO_CONTENT)
async def update_recipe(recipe_update_dto: schemas.RecipeDto, current_user: datamodels.User = Depends(get_current_user)) -> None:
    print(current_user)
    await repository.update_recipe(recipe=recipe_update_dto)


@router.delete("/recipes/{recipe_id}", status_code=status.HTTP_204_NO_CONTENT)
async def delete_recipe(recipe_id: int, current_user: datamodels.User = Depends(get_current_user)) -> None:
    print(current_user)
    await repository.delete_recipe(id=recipe_id)
