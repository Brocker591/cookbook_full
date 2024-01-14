from fastapi import FastAPI, Response, status, HTTPException, Depends, APIRouter
from fastapi.security import OAuth2PasswordRequestForm
from src.repositories import user_repository, session
from sqlalchemy.orm import Session
from src import schemas


router = APIRouter(tags=["Users"])


@router.post("/users", status_code=201)
async def create_user_endpoint(user_create_dto: schemas.UserBaseDto, db: Session = Depends(session.get_session)):
    return await user_repository.create_user(user_create_dto=user_create_dto, db=db)


@router.post("/login", status_code=200)
async def login_user_endpoint(form_data: OAuth2PasswordRequestForm = Depends(), db: Session = Depends(session.get_session)):
    return await user_repository.login_user(form_data=form_data, db=db)
