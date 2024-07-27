from sqlalchemy import Column, Integer, String, Boolean, ForeignKey, event
from sqlalchemy.sql.expression import text
from sqlalchemy.orm import Mapped
from sqlalchemy.orm import mapped_column
from sqlalchemy.orm import DeclarativeBase
from sqlalchemy.orm import relationship
from passlib.context import CryptContext
from typing import List
from app.repositories import session
from app.config import ADMIM_PASSWORD


class User(session.Base):
    __tablename__ = "users"
    id = Column(Integer, primary_key=True, nullable=False, autoincrement=True)
    username = Column(String, nullable=False)
    email = Column(String, nullable=False)
    password = Column(String, nullable=False)


class Recipe(session.Base):
    __tablename__ = "recipes"
    id = Column(Integer, primary_key=True, nullable=False, autoincrement=True)
    name = Column(String, nullable=False)
    preparation = Column(String, nullable=False)
    ingredients: Mapped[List["Ingredient"]] = relationship(
        back_populates="recipe", cascade="all,delete")


class Ingredient(session.Base):
    __tablename__ = "ingredients"
    id = Column(Integer, primary_key=True, nullable=False, autoincrement=True)
    name = Column(String, nullable=False)
    quantity = Column(String, nullable=True)

    recipe_id: Mapped[int] = mapped_column(ForeignKey("recipes.id"))
    recipe: Mapped["Recipe"] = relationship(back_populates="ingredients")


class Item(session.Base):
    __tablename__ = "items"
    id = Column(Integer, primary_key=True, nullable=False, autoincrement=True)
    name = Column(String, nullable=False, unique=True)
    quantity = Column(String, nullable=True)
    priority = Column(Integer, nullable=False, default=0)
    inventory = Column(Boolean, nullable=False, default=False)


class MealPlan(session.Base):
    __tablename__ = "mealplans"
    id = Column(String, primary_key=True, nullable=False, unique=True)
    monday = Column(String, nullable=True)
    tuesday = Column(String, nullable=True)
    wednesday = Column(String, nullable=True)
    thursday = Column(String, nullable=True)
    friday = Column(String, nullable=True)
    saturday = Column(String, nullable=True)
    sunday = Column(String, nullable=True)


@event.listens_for(User.__table__, 'after_create')
def insert_initial_user(*args, **kwargs):

    db_session = session.SessionLocal()
    pwd_context = CryptContext(schemes=["bcrypt"])
    user = User(username='admin', email='admin@example.com',
                password=pwd_context.hash(ADMIM_PASSWORD))
    db_session.add(user)
    db_session.commit()
    db_session.close()
