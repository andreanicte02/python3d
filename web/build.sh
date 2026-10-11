#!/usr/bin/env bash
# Corre el compilador original sobre ejemplos/ y arma la pagina en web/publicar/.
# Requiere el SDK de .NET 8.
set -euo pipefail

cd "$(dirname "$0")/.."
rm -rf web/publicar
mkdir -p web/publicar
dotnet run --project Generador -c Release -- ejemplos Sec/api.c web/publicar/resultados.json
cp web/index.html web/app.js web/estilos.css web/publicar/
echo "Listo: web/publicar"
