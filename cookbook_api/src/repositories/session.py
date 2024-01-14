from sqlalchemy import create_engine
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker


SQLIRW_DATABASE_URL = "sqlite:///cookbook.db"
engine = create_engine(SQLIRW_DATABASE_URL, connect_args={
                       "check_same_thread": False})
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
Base = declarative_base()

# Erstellen der Dependency  Methode gibt eine Datenbank Session zurück


def get_session():
    session = SessionLocal()
    try:
        yield session
    finally:
        session.close()
