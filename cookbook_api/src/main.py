from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from src.routers.recipe_router import router as recipe_router
from src.routers.user_router import router as user_router
from src.repositories import session
import src.config as config



session.Base.metadata.create_all(bind=session.engine)


app = FastAPI()

origins = [
    config.CORS_ORIGIN_URL,
]

app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

app.include_router(recipe_router)
app.include_router(user_router)
