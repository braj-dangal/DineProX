Hostinger VPS Deployment Guide for DineProX

1. Prerequisites
- Docker and Docker Compose installed on the VPS.
- A domain or public IP for the app.
- A PostgreSQL password and app URL.

2. Prepare environment variables
On the VPS, create a `.env` file in the repository root:

```env
POSTGRES_PASSWORD=change_me
APP_SELF_URL=http://YOUR_PUBLIC_IP:8080
APP_CORS_ORIGINS=http://YOUR_PUBLIC_IP:8080,http://localhost:4200
```

3. Build and run
```bash
docker compose -f docker-compose.prod.yml up --build -d
```

4. Verify
```bash
docker compose -f docker-compose.prod.yml ps
curl http://127.0.0.1:8080/swagger/index.html
```

5. Reverse proxy (optional)
If using Nginx or Hostinger’s proxy panel, forward:
- `80/443` to the Docker container port `8080`.

6. Notes
- Replace `YOUR_PUBLIC_IP` or your domain in the env file.
- For production HTTPS, place a reverse proxy in front of the container or use Hostinger’s SSL/TLS configuration.
- For local-network-only deployment, set the app URL to the internal LAN IP and keep the container port open on the VPS firewall.
