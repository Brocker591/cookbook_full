from dotenv import load_dotenv
import os

load_dotenv()


CORS_ORIGIN_URL = os.getenv('CORS_ORIGIN_URL')
API_KEY = os.getenv('API_KEY')
ADMIM_PASSWORD = os.getenv('ADMIM_PASSWORD')