from pydantic import BaseModel, EmailStr
from typing import Optional, List


class IngredientBaseDto(BaseModel):
    name: str
    quantity: str


class IngredientDto(IngredientBaseDto):
    recipe_id: int


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
