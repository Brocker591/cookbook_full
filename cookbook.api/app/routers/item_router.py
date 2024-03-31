from fastapi import FastAPI, Response, status, HTTPException, Depends, APIRouter
from sqlalchemy.orm import Session
from typing import Optional, List
from app import schemas
from app.auth import get_current_user
from app.repositories import datamodels
from app.repositories import session
from app.repositories import item_repository as repository


router = APIRouter(tags=["Items"])


@router.get("/items", status_code=status.HTTP_200_OK, response_model=List[schemas.ItemDto])
async def get_all_items(db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> List[schemas.ItemDto]:
    items = await repository.get_all_items(db=db)
    return items


@router.get("/items/{item_id}", status_code=status.HTTP_200_OK, response_model=schemas.ItemDto)
async def get_item(item_id: int, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> schemas.ItemDto:
    item = await repository.get_item(item_id=item_id, db=db)
    return item


@router.post("/items", status_code=status.HTTP_201_CREATED, response_model=schemas.ItemDto)
async def create_item(item: schemas.ItemBaseDto, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> schemas.ItemDto:
    new_item = await repository.create_item(item=item, db=db)
    return new_item


@router.put("/items", status_code=status.HTTP_204_NO_CONTENT)
async def update_item(item: schemas.ItemDto, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> None:
    await repository.update_item(item=item, db=db)
    return status.HTTP_204_NO_CONTENT


@router.delete("/items/{item_id}", status_code=status.HTTP_204_NO_CONTENT)
async def delete_item(item_id: int, db: Session = Depends(session.get_session), current_user: datamodels.User = Depends(get_current_user)) -> None:
    await repository.delete_item(item_id=item_id, db=db)
    return status.HTTP_204_NO_CONTENT
