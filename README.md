<p align="center">
  <img src="header.png" alt="Spatial" />
  <br/>
  <br/>
</p>

<p align="center">
  <a href="https://github.com/sptlco/spatial/actions/workflows/build.yml"><img src="https://github.com/sptlco/spatial/actions/workflows/build.yml/badge.svg" alt="Build" /></a>
  <a href="LICENSE"><img src="https://img.shields.io/github/license/sptlco/spatial" alt="License" /></a>
  <br/>
  <br/>
</p>

This repository contains all official source code and documentation for [Spatial Corporation](https://sptlco.com) — including the public website, company blog, and the private platform powering its operations. At its core, Spatial (The Platform) is an autonomous, self-learning system that perceives its environment, reasons over time, and takes action, running continuously in the cloud.

---

| System                                            | Status        |
| ------------------------------------------------- | ------------- |
| Identity (users, roles, permissions, API keys)    | `working`     |
| ECS simulation engine                             | `working`     |
| Neural network (Hypersolver)                      | `working`     |
| Compute scheduler                                 | `working`     |
| Logistics — assets (Ethereum)                     | `working`     |
| Logistics — shipments, orders, inventory (Stripe) | `in progress` |
| TCP networking                                    | `working`     |
| Web dashboard                                     | `in progress` |
| Mobile app                                        | `in progress` |
| Compute interface                                 | `planned`     |

---

## Requirements

- [Docker](https://docs.docker.com/get-docker/) with the Compose plugin
- [.NET 10 SDK](https://dotnet.microsoft.com/download) (development only)
- [Node.js 20](https://nodejs.org/) (development only)

## Getting started

Run the setup script once to install the CLI:

```bash
bash scripts/setup.sh
```

Then deploy the full stack:

```bash
spatial deploy
```

## CLI

```
spatial deploy      Build and start all services
spatial shutdown    Stop all services
spatial redeploy    Rebuild and restart, then reload NGINX
spatial develop     Run the cloud server in watch mode (hot reload)
spatial status      Show running containers
spatial logs        Tail logs from all services
```

## Services

| Service | Description                                           |
| ------- | ----------------------------------------------------- |
| `cloud` | .NET 10 backend — ECS simulation, API, neural network |
| `web`   | Next.js frontend — platform dashboard                 |
| `mongo` | MongoDB 8 — primary data store                        |
| `redis` | Redis 8 — caching and ephemeral state                 |
| `nginx` | Reverse proxy — routes traffic, terminates TLS        |
| `code`  | code-server — browser-based VS Code for development   |

## Configuration

The cloud server uses layered ASP.NET configuration. Settings are merged in this order:

1. `cloud/Server/appsettings.json` — base defaults
2. `cloud/Server/appsettings.{Environment}.json` — environment overrides (`Development`, `Production`)
3. `cloud/Server/appsettings.Override.json` — local secrets (not committed)

Create `appsettings.Override.json` and populate it before running:

```json
{
  "JWT": {
    "Issuer": "https://your-domain.com",
    "Audience": "https://your-domain.com",
    "Secret": "<signing-secret>"
  },
  "Database": {
    "ConnectionString": "mongodb://mongo:27017",
    "Name": "spatial"
  },
  "Cache": {
    "ConnectionString": "redis:6379",
    "Database": 0
  },
  "Ethereum": {
    "Endpoint": "https://mainnet.infura.io/v3/<api-key>",
    "Key": "<wallet-private-key>",
    "Allocator": {
      "Exposure": 0.0,
      "Bandwidth": 0.0,
      "Minimum": 0,
      "Deadline": 0,
      "Tolerance": 0.0
    }
  },
  "Stripe": {
    "Key": "sk_live_..."
  },
  "SMTP": {
    "Host": "mail.example.com",
    "Port": 587,
    "Name": "Spatial",
    "Username": "system@example.com",
    "Password": "<password>"
  }
}
```

| Key                            | Description                                   |
| ------------------------------ | --------------------------------------------- |
| `JWT.Secret`                   | Signing secret for issued tokens              |
| `Database.ConnectionString`    | MongoDB connection string                     |
| `Cache.ConnectionString`       | Redis connection string                       |
| `Ethereum.Endpoint`            | Ethereum RPC endpoint (e.g. Infura)           |
| `Ethereum.Key`                 | Wallet private key used by the allocator      |
| `Ethereum.Allocator.Exposure`  | Max fraction of holdings exposed per position |
| `Ethereum.Allocator.Bandwidth` | Max fraction of holdings traded per tick      |
| `Ethereum.Allocator.Minimum`   | Minimum trade size in USD                     |
| `Ethereum.Allocator.Deadline`  | Trade execution deadline in seconds           |
| `Ethereum.Allocator.Tolerance` | Slippage tolerance                            |
| `Stripe.Key`                   | Stripe secret key for commerce operations     |
| `SMTP.*`                       | Outbound mail credentials                     |

### Web

The web interface is configured via `interface/web/.env`:

```env
NEXT_PUBLIC_SERVER_ENDPOINT=https://your-domain.com
JWT_SECRET=<same-secret-as-server>
```

| Key                           | Description                                  |
| ----------------------------- | -------------------------------------------- |
| `NEXT_PUBLIC_SERVER_ENDPOINT` | URL of the cloud server API                  |
| `JWT_SECRET`                  | Must match `JWT.Secret` in the server config |


To start the web development server, run the following:
```bash
cd interface
npm run dev:web
```

### NGINX

`config/nginx/default.conf` is the NGINX virtual host configuration. Update the `server_name` directives to match your domain, and ensure the `ssl_certificate` and `ssl_certificate_key` paths point to your certificates:

```
config/nginx/ssl/certs/<domain>.pem
config/nginx/ssl/private/<domain>.key
```

SSL certificates must be present before the `nginx` container will start. The recommended way to obtain them is with [Certbot](https://certbot.eff.org/), which is available on Linux, macOS, and Windows (via WSL 2):

```bash
certbot certonly --standalone -d your-domain.com -d cloud.your-domain.com -d code.your-domain.com
```

Once issued, place the certificate files at these paths:

```
config/nginx/ssl/certs/your-domain.com.pem   ← fullchain.pem
config/nginx/ssl/private/your-domain.com.key  ← privkey.pem
```

The default config expects four subdomains:

| Subdomain               | Proxies to           |
| ----------------------- | -------------------- |
| `your-domain.com`       | `web` — public site  |
| `cloud.your-domain.com` | `cloud` — API        |
| `code.your-domain.com`  | `code` — code-server |

It is also recommended to restrict the `code` subdomain to your IP address in `default.conf`:

```nginx
server {
    listen 443 ssl http2;
    server_name code.your-domain.com;

    allow <your-ip>;
    deny all;

    ...
}
```

### code-server

`config/code/config.yaml` configures the browser-based editor. Set a strong password before deploying:

```yaml
bind-addr: 0.0.0.0:8080
auth: password
password: <your-password>
cert: false
```

## Architecture

```
interface/
  web/        Next.js app — public site, blog, and platform dashboard
  mobile/     React Native app (Expo)

cloud/
  Core/       Foundation library (ECS, networking, identity, compute)
  Server/     Cloud server (API, ECS systems, data models)
  Generators/ Compile-time source generators for ECS types
  Tests/      Unit tests
  Performance/ Benchmarks
```

## Key systems

### Neural network

The brain of the system. Neurons and synapses live as ECS entities and are updated each tick.

| Neuron type | Role                                                                          |
| ----------- | ----------------------------------------------------------------------------- |
| Sensory     | Receives preprocessed feature vectors from external protocols                 |
| Temporal    | Maintains time-dependent state via RK4 integration                            |
| Command     | Smooths temporal dynamics into higher-level signals via exponential filtering |
| Motor       | Emits output signals routed to protocol-specific Propagators                  |

Synaptic weights evolve continuously using Hebbian plasticity modulated by spatial distance and activity. Learned state is flushed to MongoDB on shutdown and restored on startup.

Signal flow: `Protocol → Sensory → Temporal → Command → Motor → Protocol`

The temporal dynamics are based on Liquid Time-constant (LTC) networks. See [references](#references) for the foundational papers.

### ECS engine

A custom high-performance Entity Component System (ECS).

- Archetype-based chunk storage for cache-friendly iteration
- Type-safe Queries, Signatures, and Futures via compile-time source generators
- Parallel mutation via `Space.Mutate` with `Future` scheduling

### Compute

A custom job scheduler with work-stealing parallelism and GPU acceleration via [ILGPU](https://ilgpu.net/).

- **Computer** — manages a pool of `Agent` workers, one per logical CPU core
- **Agent** — each runs on a dedicated highest-priority thread and steals jobs from peers when idle (see [references](#references))
- **Graph** — a DAG of jobs topologically sorted via Khan's algorithm before dispatch; cycles are detected and rejected
- **Handle** — an awaitable completion token returned on dispatch

| Job type           | Description                                                        |
| ------------------ | ------------------------------------------------------------------ |
| `CommandJob`       | A single callable with configurable timeout                        |
| `ParallelForJob`   | Splits an iteration range into batches distributed across agents   |
| `ParallelFor2DJob` | Same, over a 2D index space                                        |
| `KernelJob`        | GPU-accelerated kernel; targets CUDA, OpenCL, or CPU automatically |

A web interface for submitting and monitoring compute jobs is planned.

### Identity

Full role-based access control.

- JWT sessions with custom claims enrichment
- Users, accounts, roles, permissions, scopes, and API keys
- Assignments bind roles to principals with optional scope restrictions

### Logistics

The logistics module covers two distinct areas:

**Assets** track the configured wallet's Ethereum holdings. The dashboard shows current balance, transaction count, total volume (USD), gas costs, and performance over configurable time windows (24h, 7d, 30d, 1y). Transactions are executed autonomously to maintain the configured exposure (by swapping stable coin `USDC` for `ETH` and vice versa).

**Shipments, orders, and inventory** are Stripe-based commerce operations — physical goods, fulfillment, and sales management. The dashboard provides a searchable, filterable shipment list with origin and destination details. Order tracking and inventory management are in progress.

### TCP networking

A custom binary protocol for native client communication.

- Packets are framed with a variable-length prefix (1 or 3 bytes)
- Payloads are encrypted with a seeded XOR cipher established via handshake
- `NETCOMMAND` identifies message types; `ProtocolBuffer` handles serialization

> The networking layer is inspired by the protocol design of [Fiesta Online](https://en.wikipedia.org/wiki/Fiesta_Online), and originally powered a server emulator for that game.

## Stack

| Layer    | Technology                        |
| -------- | --------------------------------- |
| Backend  | C# / .NET 10, ASP.NET Core        |
| Database | MongoDB 8, Redis 8                |
| Ethereum | Nethereum                         |
| Payments | Stripe                            |
| Logging  | Serilog                           |
| Web      | Next.js, TypeScript, Tailwind CSS |
| Mobile   | React Native, Expo                |

## Contributing

Contributions are appreciated!\
All changes to this repository require a pull request. Direct pushes to `main` are not permitted.\
See the [backlog](https://github.com/orgs/sptlco/projects/1) for current issues.

### Workflows

| Workflow    | Trigger              | What it does                                                                     |
| ----------- | -------------------- | -------------------------------------------------------------------------------- |
| Build       | Push to any branch   | Restores, builds, and runs tests                                                 |
| Performance | PR or push to `main` | Runs benchmarks and uploads results as artifacts (retained for 90 days)          |
| Release     | Push to `main`       | Creates a release via release-please, then deploys to production (upon approval) |
| Publish     | Version tag          | Publishes packages to GitHub Packages                                            |

Benchmark results are attached to each workflow run and can be compared across PRs to catch regressions before they land.

### Deployment

Merging to `main` triggers an automatic deployment to **West US** ([sptlco.com](https://sptlco.com)) via SSH once release-please determines a release is warranted.

### Packages

The following packages are published to GitHub Packages on release:

| Package                | Description                                       |
| ---------------------- | ------------------------------------------------- |
| `Spatial` (NuGet)      | Core library — ECS, compute, networking, identity |
| `@sptlco/client` (npm) | API client for the web and mobile interfaces      |
| `@sptlco/data` (npm)   | Shared data types and schemas                     |
| `@sptlco/design` (npm) | Design system and component library               |

## References

- Hasani, R., et al. — [Liquid Time-constant Recurrent Neural Networks as Universal Approximators](https://arxiv.org/pdf/1811.00321)
- Hasani, R., et al. — [Liquid Time-constant Networks](https://arxiv.org/pdf/2006.04439)
- Chase Latta — [Liquid Neural Networks](https://www.youtube.com/watch?v=IlliqYiRhMU&t=1970s) (lecture)
- D. Chase, D. Lev — [Dynamic Circular Work-Stealing Deque](https://www.dre.vanderbilt.edu/~schmidt/PDF/work-stealing-dequeue.pdf)

For a discussion of how these ideas are applied in this codebase, see [Notes on Temporal ECS Neural Dynamics](https://dakarai.org/blog/post/temporal-ecs-neural-dynamics).

## License

Copyright &copy; Spatial Corporation. See [LICENSE](LICENSE) for details.
