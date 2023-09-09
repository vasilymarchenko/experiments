
REM docker-compose build
REM docker-compose -f docker-compose.yml -f docker-compose.override.yml -f docker-compose.infra-prometheus.yml up

docker-compose -f docker-compose.infra-prometheus.yml build
docker-compose -f docker-compose.infra-prometheus.yml up