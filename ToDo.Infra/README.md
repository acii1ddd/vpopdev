# ToDo.Infrastructure

High-availability PostgreSQL cluster infrastructure based on Patroni with HAProxy for load balancing and Prometheus for monitoring.

## 📋 Components

| Component | Version | Description |
|-----------|---------|-------------|
| PostgreSQL | 17.0 | Database |
| Patroni | latest | Template for HA PostgreSQL |
| etcd | 3.5.18 | DCS (Distributed Configuration Store) |
| HAProxy | 3.1.3 | Load Balancer |
| Prometheus | latest | Monitoring system |
| Grafana | latest | Metrics visualization |

## 🏗️ Architecture

```
                    ┌─────────────────────────────────────────────────┐
                    │              HAProxy                            │
                    │  Port 5432 → Master (write)                     │
                    │  Port 5433 → Replicas (read, round-robin)       │
                    │  Port 8080 → Stats UI                           │
                    └─────────────────────────────────────────────────┘
                                      │
          ┌───────────────────────────┼───────────────────────────┐
          │                           │                           │
    ┌─────▼─────┐             ┌──────▼──────┐             ┌──────▼──────┐
    │ Patroni-1 │             │ Patroni-2   │             │ Patroni-3   │
    │ (PG 5432) │             │  (PG 5432)  │             │  (PG 5432)  │
    │ (API 8008)│             │  (API 8008) │             │  (API 8008) │
    └─────┬─────┘             └──────┬──────┘             └──────┬──────┘
          │                          │                           │
          └──────────────────────────┼───────────────────────────┘
                                     │
                    ┌────────────────▼─────────────────┐
                    │             etcd Cluster         │
                    │   etcd1 : etcd2 : etcd3          │
                    │   (2379/2380)                    │
                    └──────────────────────────────────┘
```

## 🗄️ PostgreSQL Cluster

### Instances

| Instance | Container | Hostname | PG Port | API Port | Role |
|----------|-----------|----------|---------|----------|------|
| Node 1 | `patroni-1` | `patroni-1` | 5432 | 8008 | Master/Replica |
| Node 2 | `patroni-2` | `patroni-2` | 5432 | 8008 | Master/Replica |
| Node 3 | `patroni-3` | `patroni-3` | 5432 | 8008 | Master/Replica |

**Note:** Node role (master/replica) is determined dynamically via Patroni and etcd.

### Replication

- **Type:** Asynchronous streaming replication
- **Replication user:** `replicator`
- **Mechanism:** Patroni automatically manages master election and failover

## 🔄 HAProxy

### Ports

| Port | Purpose |
|------|---------|
| 5432 | Write (master only) |
| 5433 | Read (all replicas, round-robin) |
| 8080 | Stats Web UI (admin:admin) |

### Health Checks

HAProxy uses Patroni REST API to determine node health:
- `/master` — active master check
- `/replica` — replica check

## 📊 Monitoring

| Service | Port | Description |
|---------|------|-------------|
| Prometheus | 9090 | Metrics collection |
| Grafana | 3000 | Dashboards |
| HAProxy Exporter | 9101 | HAProxy metrics |

## 🚀 Quick Start

### Requirements

- Docker and Docker Compose
- Environment variable `SECRET` for etcd cluster token

### Installation

```bash
# Clone repository
git clone <repository-url>
cd ToDo.Infra

# Set SECRET variable (for etcd)
export SECRET=$(openssl rand -hex 16)

# Start all services
docker compose up -d
```

### Check Status

```bash
# Container status
docker compose ps

# Patroni logs
docker compose logs -f patroni-1

# Check cluster via Patroni API
http://localhost:8080/master
http://localhost:8080/replica
```

# View HAProxy metrics
curl http://localhost:8080/;csv

## 🔧 Configuration

### Patroni Environment Variables

| Variable | Default | Description |
|----------|---------|-------------|
| `PATRONI_API_CONNECT_PORT` | 8008 | Patroni API port |
| `PATRONI_REPLICATION_USERNAME` | replicator | Replication user |
| `PATRONI_REPLICATION_PASSWORD` | replpass | Replication password |
| `PATRONI_SUPERUSER_USERNAME` | postgres | Superuser |
| `PATRONI_SUPERUSER_PASSWORD` | supass | Superuser password |

### Data Volumes

| Volume | Instance | Purpose |
|--------|----------|---------|
| `test-data-1` | patroni-1 | PostgreSQL data |
| `test-data-2` | patroni-2 | PostgreSQL data |
| `test-data-3` | patroni-3 | PostgreSQL data |

## 🔍 Useful Commands

```bash
# Restart specific node
docker compose restart patroni-1

# Connect to PostgreSQL (via HAProxy)
psql -h localhost -p 5432 -U postgres  # write
psql -h localhost -p 5433 -U postgres  # read
```

## 📁 File Structure

```
ToDo.Infra/
├── compose.yml           # Docker Compose configuration
├── Dockerfile            # Patroni + PostgreSQL image
├── entrypoint.sh         # Container initialization script
├── haproxy.cfg           # HAProxy configuration
├── patroni.yml           # Patroni configuration
├── prometheus.yml        # Prometheus configuration
├── scripts/
│   ├── init_master.sh    # Master initialization
│   ├── init_slave.sh     # Replica initialization
│   └── post_bootstrap.sh # Post-bootstrap script
└── README.md             # This file
```

## ⚠️ Important Notes

1. **etcd cluster:** Requires minimum 3 nodes for quorum
2. **Failover:** Automatic on master failure (configurable in patroni.yml)
3. **Persistence:** Data stored in Docker volumes
4. **Network:** All services in isolated network `devpops2`
