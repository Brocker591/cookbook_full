from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from app.routers.recipe_router import router as recipe_router
from app.routers.inventory_router import router as inventory_router
from app.routers.user_router import router as user_router
from app.routers.item_router import router as item_router
from app.routers.meal_plan_router import router as meal_plan_router
from app.repositories import session
from app import config


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
app.include_router(item_router)
app.include_router(inventory_router)
app.include_router(meal_plan_router)


@app.get("/health")
def healthcheack():
    return {"status": "healthy"}
