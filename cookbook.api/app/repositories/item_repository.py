from sqlalchemy.orm import Session
from fastapi import Depends, HTTPException, status
from typing import List
from app.schemas import ItemDto, ItemBaseDto
from app.repositories.datamodels import Item



async def get_all_items(db: Session) -> List[ItemDto]:
    db_items = db.query(Item).all()

    all_items_dto = [
        ItemDto(id=x.id, name=x.name, quantity=x.quantity, priority=x.priority, inventory=x.inventory) for x in db_items]
    return all_items_dto


async def get_item_by_id(item_id: int, db: Session) -> ItemDto:
    db_item = await get_datamodel_Item_by_id(item_id=item_id, db=db)
    item_dto = ItemDto(id=db_item.id, name=db_item.name, quantity=db_item.quantity, priority=db_item.priority, inventory=db_item.inventory)
    
    return item_dto


async def create_item(item: ItemBaseDto, db: Session) -> ItemDto:
    exist_item = db.query(Item).filter(Item.name == item.name).first()

    if exist_item:
        raise HTTPException(status_code=status.HTTP_400_BAD_REQUEST, detail="Item already exists")

    new_item = Item(name=item.name, quantity=item.quantity, inventory=True)
    db.add(new_item)
    db.commit()
    db.refresh(new_item)
    item_dto = ItemDto(id=new_item.id, name=new_item.name, quantity=new_item.quantity, priority=new_item.priority, inventory=new_item.inventory)

    return item_dto


async def update_item(item: ItemDto, db: Session) -> None:

    exist_item = await get_datamodel_Item_by_id(item_id=item.id, db=db)

    if(exist_item.inventory == False and item.inventory == True):
        exist_item.priority += 1

    exist_item.inventory = item.inventory
    exist_item.quantity = item.quantity
    exist_item.name = item.name
    db.add(exist_item)
    db.commit()


async def delete_item(item: ItemBaseDto, db: Session) -> None:

    exist_item = await get_datamodel_Item_by_id(item_id=item.id, db=db)

    db.delete(exist_item)
    db.commit()



async def get_datamodel_Item_by_id(item_id: int, db: Session) -> Item:
    db_item = db.query(Item).filter(Item.id == item_id).first()

    if not db_item:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND, detail="Item not found")
    
    return db_item