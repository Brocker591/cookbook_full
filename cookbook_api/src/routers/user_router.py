from fastapi import status, HTTPException, Depends, APIRouter, Security
from fastapi.security import OAuth2PasswordRequestForm
from fastapi.security.api_key import APIKeyHeader
from src.repositories import user_repository, session
from sqlalchemy.orm import Session
from src import schemas
import src.config as config

api_key_header = APIKeyHeader(name="x-api-key", auto_error=False)

router = APIRouter(tags=["Users"])


@router.post("/users", status_code=201)
async def create_user_endpoint(user_create_dto: schemas.UserBaseDto, db: Session = Depends(session.get_session)):
    return await user_repository.create_user(user_create_dto=user_create_dto, db=db)


@router.post("/login", status_code=200)
async def login_user_endpoint(form_data: OAuth2PasswordRequestForm = Depends(), db: Session = Depends(session.get_session)):
    return await user_repository.login_user(form_data=form_data, db=db)

@router.post("/changePassword", status_code=200)
async def change_user_password(user: schemas.UserBaseDto, api_key: str = Security(api_key_header), db: Session = Depends(session.get_session)):
    if api_key != config.API_KEY:
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Invalid API key")
    await user_repository.change_user_password(user=user, db=db)
    return status.HTTP_200_OK
