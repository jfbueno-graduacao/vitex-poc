docker compose -f .\docker-compose.yaml `
    -f .\docker-compose-nodes.yaml `
    -f .\docker-compose-replicas.yaml `
    -f .\docker-compose-replicas-extras.yaml `
    up -d --build