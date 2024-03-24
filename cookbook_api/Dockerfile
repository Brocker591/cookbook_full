FROM python:3.10

COPY ./app /app
COPY ./requirements.txt /app
COPY ./.env /app
WORKDIR /app

RUN pip install -r requirements.txt

CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "80"]
