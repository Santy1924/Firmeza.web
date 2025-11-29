#!/bin/bash
set -e

echo "Esperando a que PostgreSQL esté disponible..."
sleep 10

echo "Aplicando migraciones..."
dotnet ef database update --project /app/Firmeza.web.Data.csproj 2>/dev/null || true

echo "Iniciando Web.Api..."
exec dotnet Web.Api.dll
