from sqlalchemy.orm import Session
from fastapi import Depends, HTTPException, status
from typing import List
from app.schemas import ItemDto, ItemBaseDto, InventoryBaseDto
from app.repositories.datamodels import Item


async def add_items_to_inventory(inventory: InventoryBaseDto, db: Session) -> List[ItemDto]:

    for item in inventory.items:
        exist_item = db.query(Item).filter(Item.name == item.name).first()

        if not exist_item:
            new_item = Item(
                name=item.name, quantity=item.quantity, inventory=True)
            db.add(new_item)
            db.commit()
        else:
            exist_item.quantity = item.quantity
            exist_item.inventory = True
            exist_item.priority += 1
            db.add(exist_item)
            db.commit()

    all_inventory_items = await get_inventory(db=db)
    return all_inventory_items


async def add_item_to_inventory(item: ItemBaseDto, db: Session) -> List[ItemDto]:

    exist_item = db.query(Item).filter(Item.name == item.name).first()

    if not exist_item:
        new_item = Item(name=item.name, quantity=item.quantity, inventory=True)
        db.add(new_item)
        db.commit()

    else:
        exist_item.quantity = item.quantity
        exist_item.inventory = True
        exist_item.priority += 1
        db.add(exist_item)
        db.commit()

    all_inventory_items = await get_inventory(db=db)
    return all_inventory_items


async def create_item(item: ItemBaseDto, db: Session) -> ItemDto:

    exist_item = db.query(Item).filter(Item.name == item.name).first()

    if not exist_item:
        new_item = Item(name=item.name, quantity=item.quantity, inventory=True)
        db.add(new_item)
        db.commit()
        db.refresh(new_item)

    else:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST, detail="Item already exists")

    return ItemDto(id=new_item.id, name=new_item.name, quantity=new_item.quantity, priority=new_item.priority)


async def update_item_from_inventory(item: ItemDto, db: Session) -> None:

    exist_item = db.query(Item).filter(Item.id == item.id).first()

    if not exist_item:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND, detail="Item not found")
    else:
        exist_item.inventory = item.inventory
        exist_item.quantity = item.quantity
        exist_item.name = item.name
        db.add(exist_item)
        db.commit()


async def delete_item_from_inventory(item: ItemBaseDto, db: Session) -> List[ItemDto]:

    exist_item = db.query(Item).filter(Item.name == item.name).first()

    if not exist_item:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND, detail="Item not found")
    else:
        exist_item.inventory = False
        db.add(exist_item)
        db.commit()

    all_inventory_items = await get_inventory(db=db)
    return all_inventory_items


async def get_inventory(db: Session) -> List[ItemDto]:
    db_items = db.query(Item).filter(Item.inventory == True).all()

    all_items_dto = [
        ItemDto(id=x.id, name=x.name, quantity=x.quantity, priority=x.priority, inventory=x.inventory) for x in db_items]
    return all_items_dto


async def get_all_items(db: Session) -> List[ItemDto]:
    db_items = db.query(Item).all()

    all_items_dto = [
        ItemDto(id=x.id, name=x.name, quantity=x.quantity, priority=x.priority, inventory=x.inventory) for x in db_items]
    return all_items_dto


async def delete_item(item_id: int, db: Session) -> None:
    exist_item = db.query(Item).filter(Item.id == item_id).first()

    if not exist_item:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND, detail="Item not found")
    else:
        db.delete(exist_item)
        db.commit()
