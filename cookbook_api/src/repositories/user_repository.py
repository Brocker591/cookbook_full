from src import schemas
from src.repositories import datamodels, session
from sqlalchemy.orm import Session
from fastapi import Depends, HTTPException, status
from fastapi.security import OAuth2PasswordRequestForm
from src.auth import get_password_hash, verify_password, create_access_token, ACCESS_TOKEN_EXPIRE_IN_MINUTES


async def create_user(user_create_dto: schemas.UserBaseDto, db: Session = Depends(session.get_session)):

    existing_user = db.query(datamodels.User).filter(
        datamodels.User.email == user_create_dto.email).first()
    if existing_user:
        raise HTTPException(status_code=400, detail="Email already in use")

    user_create_dto.password = get_password_hash(user_create_dto.password)

    user = datamodels.User(username=user_create_dto.username,
                           email=user_create_dto.email, password=user_create_dto.password)
    db.add(user)
    db.commit()
    db.refresh(user)
    return {"success": f"User mit ID {user.id} erstellt"}


async def login_user(form_data: OAuth2PasswordRequestForm, db: Session = Depends(session.get_session)):
    existing_user = db.query(datamodels.User).filter(
        datamodels.User.username == form_data.username).first()

    if not existing_user:
        raise HTTPException(
            status_code=401, detail="Not able to be authenticated")

    if not verify_password(form_data.password, existing_user.password):
        raise HTTPException(
            status_code=401, detail="Not able to be authenticated")

    token = create_access_token(user=existing_user)

    return {"access_token": token, "userId": existing_user.id, "expiresIn": ACCESS_TOKEN_EXPIRE_IN_MINUTES, "token_type": "bearer"}


async def change_user_password(user: schemas.UserBaseDto, db: Session = Depends(session.get_session)) -> None:
    existing_user = db.query(datamodels.User).filter(
        datamodels.User.username == user.username).first()

    if not existing_user:
        raise HTTPException(status.HTTP_404_NOT_FOUND, detail="User not found")

    existing_user.password = get_password_hash(user.password)
    db.add(existing_user)
    db.commit()

