from fastapi import FastAPI, Response, status, HTTPException, Depends, APIRouter
from sqlalchemy.orm import Session
from typing import Optional, List
from app import schemas
from app.auth import get_current_user
from app.repositories import datamodels
from app.repositories import recipe_repository as repository
from app.repositories import session

router = APIRouter(tags=["Recipes"])


@router.get("/recipes", status_code=status.HTTP_200_OK,  response_model=List[schemas.RecipeDto])
async def get_all_recipe(db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> List[schemas.RecipeDto]:
    print(current_user)
    recipes = await repository.get_all_recipe(db=db)
    return recipes


@router.get("/recipes/{recipe_id}", status_code=status.HTTP_200_OK, response_model=schemas.RecipeDto)
async def get_recipe(recipe_id: int, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> schemas.RecipeDto:
    recipe = await repository.get_recipe(recipe_id, db=db)
    return recipe


@router.post("/recipes", status_code=status.HTTP_201_CREATED, response_model=schemas.RecipeDto)
async def create_recipe(recipe_create_Dto: schemas.RecipeCreateDto, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> schemas.RecipeDto:
    new_recipe = await repository.create_recipe(recipe=recipe_create_Dto, db=db)
    return new_recipe


@router.put("/recipes", status_code=status.HTTP_204_NO_CONTENT)
async def update_recipe(recipe_update_dto: schemas.RecipeDto, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> None:
    await repository.update_recipe(recipe=recipe_update_dto, db=db)


@router.delete("/recipes/{recipe_id}", status_code=status.HTTP_204_NO_CONTENT)
async def delete_recipe(recipe_id: int, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> None:
    await repository.delete_recipe(recipe_id=recipe_id, db=db)
