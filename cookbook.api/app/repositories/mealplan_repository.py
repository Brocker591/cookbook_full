from sqlalchemy.orm import Session
from fastapi import Depends, HTTPException, status
from typing import List
from app.schemas import MealPlanBaseDto
from app.repositories.datamodels import MealPlan


async def get_all_meal_plans(db: Session) -> List[MealPlanBaseDto]:
    db_meal_plans = db.query(MealPlan).all()

    all_meal_plans_dto = [MealPlanBaseDto(id=x.id,
                                          monday=x.monday,
                                          tuesday=x.tuesday,
                                          wednesday=x.wednesday,
                                          thursday=x.thursday,
                                          friday=x.friday,
                                          saturday=x.saturday,
                                          sunday=x.sunday)
                          for x in db_meal_plans]
    return all_meal_plans_dto


async def get_meal_plan_by_id(meal_plan_id: int, db: Session) -> MealPlanBaseDto:
    db_meal_plan = await get_datamodel_meal_plan_by_id(meal_plan_id=meal_plan_id, db=db)
    meal_plan_dto = MealPlanBaseDto(id=db_meal_plan.id, monday=db_meal_plan.monday,
                                    tuesday=db_meal_plan.tuesday, wednesday=db_meal_plan.wednesday,
                                    thursday=db_meal_plan.thursday, friday=db_meal_plan.friday,
                                    saturday=db_meal_plan.saturday, sunday=db_meal_plan.sunday)

    return meal_plan_dto


async def create_meal_plan(meal_plan: MealPlanBaseDto, db: Session) -> MealPlanBaseDto:
    exist_meal_plan = db.query(MealPlan).filter(
        MealPlan.id == meal_plan.id).first()

    if exist_meal_plan:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST, detail="Meal Plan already exists")

    new_meal_plan = MealPlan(id=meal_plan.id, monday=meal_plan.monday,
                             tuesday=meal_plan.tuesday, wednesday=meal_plan.wednesday,
                             thursday=meal_plan.thursday, friday=meal_plan.friday,
                             saturday=meal_plan.saturday, sunday=meal_plan.sunday)

    db.add(new_meal_plan)
    db.commit()
    db.refresh(new_meal_plan)
    meal_plan_dto = MealPlanBaseDto(id=new_meal_plan.id, monday=new_meal_plan.monday,
                                    tuesday=new_meal_plan.tuesday, wednesday=new_meal_plan.wednesday,
                                    thursday=new_meal_plan.thursday, friday=new_meal_plan.friday,
                                    saturday=new_meal_plan.saturday, sunday=new_meal_plan.sunday)

    return meal_plan_dto


async def update_meal_plan(meal_plan: MealPlanBaseDto, db: Session) -> None:

    exist_meal_plan = await get_datamodel_meal_plan_by_id(meal_plan_id=meal_plan.id, db=db)

    exist_meal_plan.monday = meal_plan.monday
    exist_meal_plan.tuesday = meal_plan.tuesday
    exist_meal_plan.wednesday = meal_plan.wednesday
    exist_meal_plan.thursday = meal_plan.thursday
    exist_meal_plan.friday = meal_plan.friday
    exist_meal_plan.saturday = meal_plan.saturday
    exist_meal_plan.sunday = meal_plan.sunday

    db.add(exist_meal_plan)
    db.commit()


async def delete_meal_plan(meal_plan_id: int, db: Session) -> None:

    meal_plan_item = await get_datamodel_meal_plan_by_id(meal_plan_id=meal_plan_id, db=db)

    db.delete(meal_plan_item)
    db.commit()


async def get_datamodel_meal_plan_by_id(meal_plan_id: int, db: Session) -> MealPlan:
    db_meal_plan = db.query(MealPlan).filter(
        MealPlan.id == meal_plan_id).first()

    if not db_meal_plan:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND, detail="Meal Plan not found")

    return db_meal_plan
