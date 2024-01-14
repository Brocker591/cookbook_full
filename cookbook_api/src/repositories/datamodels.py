from sqlalchemy import Column, Integer, String
from sqlalchemy.sql.expression import text
from typing import List
from src.repositories import session


class User(session.Base):
    __tablename__ = "users"
    id = Column(Integer, primary_key=True, nullable=False, autoincrement=True)
    username = Column(String, nullable=False)
    email = Column(String, nullable=False)
    password = Column(String, nullable=False)

