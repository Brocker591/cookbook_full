from fastapi import FastAPI, Response, status, HTTPException, Depends, APIRouter
from sqlalchemy.orm import Session
from typing import Optional, List
from app import schemas
from app.auth import get_current_user
from app.repositories import datamodels
from app.repositories import session
from app.repositories import inventory_repository as repository


router = APIRouter(tags=["Inventory"])


@router.post("/inventory/addList", status_code=status.HTTP_201_CREATED, response_model=schemas.InventoryBaseDto)
async def add_items_to_inventory(inventory_base: schemas.InventoryBaseDto, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> schemas.InventoryBaseDto:
    items_dto = await repository.add_items_to_inventory(inventory=inventory_base, db=db)
    new_inventory = schemas.InventoryBaseDto(items=items_dto)
    return new_inventory


@router.post("/inventory/addItem", status_code=status.HTTP_201_CREATED, response_model=schemas.InventoryBaseDto)
async def add_item_to_inventory(item: schemas.ItemBaseDto, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> schemas.InventoryBaseDto:
    item_dto = await repository.add_item_to_inventory(item=item, db=db)
    new_inventory = schemas.InventoryBaseDto(items=item_dto)
    return new_inventory


@router.post("/inventory/deleteItem", status_code=status.HTTP_201_CREATED, response_model=schemas.InventoryBaseDto)
async def delete_item_from_inventory(item: schemas.ItemBaseDto, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> schemas.InventoryBaseDto:
    item_dto = await repository.delete_item_from_inventory(item=item, db=db)
    new_inventory = schemas.InventoryBaseDto(items=[item_dto])
    return new_inventory


@router.get("/inventory", status_code=status.HTTP_200_OK, response_model=schemas.InventoryBaseDto)
async def get_inventory(db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> schemas.InventoryBaseDto:
    items = await repository.get_inventory(db=db)
    inventory = schemas.InventoryBaseDto(items=items)
    return inventory


@router.get("/items", status_code=status.HTTP_200_OK, response_model=List[schemas.ItemDto])
async def get_all_items(db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> List[schemas.ItemDto]:
    items = await repository.get_all_items(db=db)
    return items


@router.delete("/items/{item_id}", status_code=status.HTTP_204_NO_CONTENT)
async def delete_item(item_id: int, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> None:
    await repository.delete_item(item_id=item_id, db=db)
    return status.HTTP_204_NO_CONTENT

