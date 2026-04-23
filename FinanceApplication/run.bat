@echo off

echo Stopping containers...
docker-compose down -v

echo Building and starting containers...
docker-compose build --no-cache
docker-compose up

echo Done!
pause