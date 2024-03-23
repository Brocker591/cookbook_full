from pydantic import BaseModel, EmailStr, Field
from typing import Optional, List


class IngredientBaseDto(BaseModel):
    name: str
    quantity: str
    recipe_id: int


class IngredientDto(IngredientBaseDto):
    id: int = Field(default=0)


class RecipeBaseDto(BaseModel):
    description: str
    preparation: str


class RecipeCreateDto(RecipeBaseDto):
    ingredients: Optional[List[IngredientBaseDto]]


class RecipeDto(RecipeBaseDto):
    id: int
    ingredients: Optional[List[IngredientDto]]


class UserBaseDto(BaseModel):
    username: str
    email: EmailStr
    password: str


class UserDto(UserBaseDto):
    id: int
