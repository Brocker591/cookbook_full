from src.repositories import datamodels, session
from sqlalchemy.orm import Session
from fastapi import Depends, HTTPException, status
from src.schemas import ItemDto, ItemBaseDto, InventoryBaseDto
from src.repositories.datamodels import Item
from typing import List


async def add_items_to_inventory(inventory: InventoryBaseDto, db: Session) -> List[ItemDto]:

    new_db_inventory = []

    for item in inventory.items:
        exist_item = db.query(Item).filter(Item.name == item.name).first()

        if not exist_item:
            new_item = Item(
                name=item.name, quantity=item.quantity, inventory=True)
            db.add(new_item)
            db.commit()
            db.refresh(new_item)
            new_db_inventory.append(new_item)
        else:
            exist_item.quantity = item.quantity
            exist_item.inventory = True
            db.add(exist_item)
            db.commit()
            db.refresh(exist_item)
            new_db_inventory.append(exist_item)

    all_items_dto = [ItemDto(id=x.id, name=x.name, quantity=x.quantity)
                     for x in new_db_inventory]
    return all_items_dto


async def get_inventory(db: Session) -> List[ItemDto]:
    db_items = db.query(Item).filter(Item.inventory == True).all()

    all_items_dto = [
        ItemDto(id=x.id, name=x.name, quantity=x.quantity) for x in db_items]
    return all_items_dto
