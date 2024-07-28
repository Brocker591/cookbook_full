# cookbook_full

Cookbook Frontend und Backend

## Erstellen einer python virtual environment

```powershell

python -m venv .venv

```

## Aktivieren der enviroment

```powershell

 .\.venv\Scripts\activate

```

## Installieren aller Module aus der requirements.txt

```powershell

pip install -r requirements.txt

```

## Aktuell verwendete Module in requirements.txt speichern

```powershell

pip freeze > requirements.txt

```

## Swagger URL

http://127.0.0.1:8000/docs

## Install eigener Packages

Über die pyproject.toml können eigene Packages erstellt werden hierzu wird dieser Befehl benötigt

pip install -e .

## Docker

$version="1.6"

docker build -t "brocker591/cookbook.api:$version" .

docker push "brocker591/cookbook.api:$version"
