from sqlalchemy import Column, Integer, String
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


class User(session.Base):
    __tablename__ = "users"
    id = Column(Integer, primary_key=True, nullable=False, autoincrement=True)
    username = Column(String, nullable=False)
    email = Column(String, nullable=False)
    password = Column(String, nullable=False)


class Recipe(session.Base):
    __tablename__ = "recipe"
    id = Column(Integer, primary_key=True, nullable=False, autoincrement=True)
    description = Column(String, nullable=False)
    preparation = Column(String, nullable=False)
    ingredients: Mapped[List["Ingredient"]] = relationship(back_populates="recipe", cascade="all,delete")



class Ingredient(session.Base):
    __tablename__ = "ingredient"
    id = Column(Integer, primary_key=True, nullable=False, autoincrement=True)
    name = Column(String, nullable=False)
    quantity = Column(String, nullable=False)

    recipe_id: Mapped[int] = mapped_column(ForeignKey("recipe.id"))
    recipe: Mapped["Recipe"] = relationship(back_populates="ingredients")