from fastapi import FastAPI, Response, status, HTTPException, Depends, APIRouter
from sqlalchemy.orm import Session
from typing import Optional, List
from app import schemas
from app.auth import get_current_user
from app.repositories import datamodels
from app.repositories import session
from app.repositories import mealplan_repository as repository


router = APIRouter(tags=["MealPlans"])


@router.get("/mealplans", status_code=status.HTTP_200_OK, response_model=List[schemas.MealPlanBaseDto])
async def get_all_mealplans(db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> List[schemas.MealPlanBaseDto]:
    meal_plans = await repository.get_all_meal_plans(db=db)
    return meal_plans


@router.get("/mealplans/{meal_plan_id}", status_code=status.HTTP_200_OK, response_model=schemas.MealPlanBaseDto)
async def get_meal_plan(meal_plan_id: int, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> schemas.MealPlanBaseDto:
    meal_plan = await repository.get_meal_plan_by_id(meal_plan_id=meal_plan_id, db=db)
    return meal_plan


@router.post("/mealplans", status_code=status.HTTP_201_CREATED, response_model=schemas.MealPlanBaseDto)
async def create_meal_plan(meal_plan: schemas.MealPlanBaseDto, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> schemas.MealPlanBaseDto:
    new_meal_plan = await repository.create_meal_plan(meal_plan=meal_plan, db=db)
    return new_meal_plan


@router.put("/mealplans", status_code=status.HTTP_204_NO_CONTENT)
async def update_meal_plan(meal_plan: schemas.MealPlanBaseDto, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> None:
    await repository.update_meal_plan(meal_plan=meal_plan, db=db)
    return status.HTTP_204_NO_CONTENT


@router.delete("/mealplans/{meal_plan_id}", status_code=status.HTTP_204_NO_CONTENT)
async def delete_item(meal_plan_id: int, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> None:
    await repository.delete_meal_plan(meal_plan_id=meal_plan_id, db=db)
    return status.HTTP_204_NO_CONTENT
