from fastapi import FastAPI
from src.routers.recipe_router import router as recipe_router
from src.routers.user_router import router as user_router
from src.repositories import session

session.Base.metadata.create_all(bind=session.engine)

app = FastAPI()

app.include_router(recipe_router)
app.include_router(user_router)
