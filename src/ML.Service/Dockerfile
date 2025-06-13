FROM python:3.11-slim

WORKDIR /app

COPY ./ML.Service/requirements.txt .

RUN pip install --no-cache-dir -r requirements.txt

COPY . .

EXPOSE 2500

CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "2500", "--reload"]