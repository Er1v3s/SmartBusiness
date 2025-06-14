FROM python:3.11-slim

# Ustawienie katalogu roboczego w kontenerze
WORKDIR /app

# Skopiowanie pliku requirements.txt do kontenera
COPY ML.Service/requirements.txt /app/

# Instalacja zale�no�ci
RUN pip install --no-cache-dir -r /app/requirements.txt

# Skopiowanie reszty aplikacji
COPY ML.Service /app/

# Ustawienie katalogu roboczego na /app
WORKDIR /app

# Uruchomienie aplikacji FastAPI
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "2500"]