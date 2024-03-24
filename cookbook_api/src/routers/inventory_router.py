from fastapi import FastAPI, Response, status, HTTPException, Depends, APIRouter
from src.repositories import inventory_repository as repository
from src.repositories import session
from sqlalchemy.orm import Session
from typing import Optional, List
from src import schemas
from src.auth import get_current_user
from src.repositories import datamodels


router = APIRouter(tags=["Inventories"])


@router.post("/inventories", status_code=status.HTTP_201_CREATED, response_model=schemas.InventoryBaseDto)
async def add_items_to_inventory(inventory_base: schemas.InventoryBaseDto, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> schemas.InventoryBaseDto:
    items_dto = await repository.add_items_to_inventory(inventory=inventory_base, db=db)
    new_inventory = schemas.InventoryBaseDto(items=items_dto)
    return new_inventory


@router.get("/inventories", status_code=status.HTTP_200_OK, response_model=schemas.InventoryBaseDto)
async def get_inventory(db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> schemas.InventoryBaseDto:
    items = await repository.get_inventory(db=db)
    inventory = schemas.InventoryBaseDto(items=items)
    return inventory
