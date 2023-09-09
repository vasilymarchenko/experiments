docker-compose build

REM docker-compose -f docker-compose.yml -f docker-compose.override.yml -f docker-compose.infra.yml up
docker-compose -f docker-compose.yml -f docker-compose.override.yml -f docker-compose.infra-prometheus.yml up