# Docker Setup para Firmeza.web

## Requisitos
- Docker instalado
- Docker Compose instalado

## Instrucciones de uso

### 1. Construir y ejecutar con Docker Compose

```bash
# Desde la raíz de la solución
docker-compose up --build
```

Esto iniciará:
- **PostgreSQL** en puerto 5432
- **Web.Api** en puerto 5272
- **Cliente (React)** en puerto 5173

### 2. Acceder a los servicios

- **Frontend**: http://localhost:5173
- **Web.Api Swagger**: http://localhost:5272/swagger
- **Base de datos**: localhost:5432 (postgres / Qwe.123*)

### 3. Detener los servicios

```bash
docker-compose down
```

### 4. Reconstruir imágenes

```bash
docker-compose build --no-cache
docker-compose up
```

### 5. Ver logs

```bash
# Todos los servicios
docker-compose logs -f

# Servicio específico
docker-compose logs -f web-api
docker-compose logs -f cliente
docker-compose logs -f postgres
```

## Variables de entorno

El archivo `docker-compose.yml` configura automáticamente:
- **Credenciales de BD**: postgres / Qwe.123*
- **URL de API**: http://web-api:5272/api
- **Ambiente**: Development

## Notas

- Los datos de PostgreSQL se almacenan en un volumen Docker (`postgres_data`)
- Las migraciones se ejecutan automáticamente al iniciar Web.Api
- El frontend se sirve con `serve` en producción
