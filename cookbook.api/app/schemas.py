from pydantic import BaseModel, EmailStr, Field
from typing import Optional, List
from pydantic import BaseModel


class IngredientBaseDto(BaseModel):
    name: str
    quantity: Optional[str] = None
    recipe_id: int


class IngredientDto(IngredientBaseDto):
    id: int = Field(default=0)


class RecipeBaseDto(BaseModel):
    name: str
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


class ItemBaseDto(BaseModel):
    name: str
    quantity: Optional[str] = None


class ItemDto(ItemBaseDto):
    id: int = Field(default=0)
    priority: Optional[int] = 0
    inventory: bool = Field(default=False)


class InventoryBaseDto(BaseModel):
    items: List[ItemDto]
