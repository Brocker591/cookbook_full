from sqlalchemy import Column, Integer, String, Boolean
from sqlalchemy.sql.expression import text
from typing import List
from src.repositories import session

from sqlalchemy import ForeignKey
from sqlalchemy import Integer
from sqlalchemy.orm import Mapped
from sqlalchemy.orm import mapped_column
from sqlalchemy.orm import DeclarativeBase
from sqlalchemy.orm import relationship
from src.repositories import session
from sqlalchemy.orm import Mapped
from sqlalchemy import event
from passlib.context import CryptContext
from fastapi.security import OAuth2PasswordBearer


class User(session.Base):
    __tablename__ = "users"
    id = Column(Integer, primary_key=True, nullable=False, autoincrement=True)
    username = Column(String, nullable=False)
    email = Column(String, nullable=False)
    password = Column(String, nullable=False)


class Recipe(session.Base):
    __tablename__ = "recipes"
    id = Column(Integer, primary_key=True, nullable=False, autoincrement=True)
    description = Column(String, nullable=False)
    preparation = Column(String, nullable=False)
    ingredients: Mapped[List["Ingredient"]] = relationship(
        back_populates="recipe", cascade="all,delete")


class Ingredient(session.Base):
    __tablename__ = "ingredients"
    id = Column(Integer, primary_key=True, nullable=False, autoincrement=True)
    name = Column(String, nullable=False)
    quantity = Column(String, nullable=False)

    recipe_id: Mapped[int] = mapped_column(ForeignKey("recipes.id"))
    recipe: Mapped["Recipe"] = relationship(back_populates="ingredients")


class Item(session.Base):
    __tablename__ = "items"
    id = Column(Integer, primary_key=True, nullable=False, autoincrement=True)
    name = Column(String, nullable=False, unique=True)
    quantity = Column(String, nullable=True)
    inventory = Column(Boolean, nullable=False, default=False)




@event.listens_for(User.__table__, 'after_create')
def insert_initial_user(*args, **kwargs):

    db_session = session.SessionLocal()
    pwd_context = CryptContext(schemes=["bcrypt"])
    user = User(username='admin', email='admin@example.com',
                password=pwd_context.hash("admin"))
    db_session.add(user)
    db_session.commit()
    db_session.close()
