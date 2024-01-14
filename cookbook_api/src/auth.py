from passlib.context import CryptContext
from fastapi.security import OAuth2PasswordBearer
from src.repositories import datamodels, session
import datetime
from jose import jwt, JWTError
from fastapi import Depends, HTTPException
from sqlalchemy.orm import Session

SECRET_KEY = "123456"
ALGORITHM = "HS256"
ACCESS_TOKEN_EXPIRE_IN_MINUTES = 30


pwd_context = CryptContext(schemes=["bcrypt"])
# weil unser Login auf unsere eigene URL /login geschrieben wurde. wird hier login angegeben
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="login")


def get_password_hash(password):
    return pwd_context.hash(password)


def verify_password(plain_password, hashed_password):
    return pwd_context.verify(secret=plain_password, hash=hashed_password)


def create_access_token(user: datamodels.User):
    try:
        claims = {
            "sub": user.username,
            "email": user.email,
            "exp": datetime.datetime.now() + datetime.timedelta(minutes=ACCESS_TOKEN_EXPIRE_IN_MINUTES)
        }
        return jwt.encode(claims, key=SECRET_KEY, algorithm=ALGORITHM)

    except JWTError:
        raise JWTError("Wrong Token")


def verify_token(token: str):
    try:
        payload = jwt.decode(token, key=SECRET_KEY, algorithms=ALGORITHM)
        return payload
    except JWTError:
        raise JWTError("Token decoding did not work!")


async def get_current_user(token: str = Depends(oauth2_scheme), db: Session = Depends(session.get_session)):
    payload = verify_token(token)
    username = payload.get("sub")
    user = db.query(datamodels.User).filter(
        datamodels.User.username == username).first()
    if not user:
        HTTPException(status_code=401, detail="Not authorized")
    return user
