# CSharp Adv – Uke 2: Docker og containere

Containerisering av et eksisterende To-do-API (TaskService) med Docker og Docker Compose. API-et lagrer data i PostgreSQL via EF Core, og trafikken går inn gjennom Traefik som reverse proxy.

## Tjenester

| Tjeneste | Image | Port (utenfra) | Beskrivelse |
|---|---|---|---|
| `traefik` | `traefik:v3.6` | `8000` (trafikk), `8081` (dashbord) | Reverse proxy som ruter trafikk til API-et |
| `api` | Bygges fra `TaskService.Api/Dockerfile` | – (kun via Traefik) | ASP.NET Core Web API |
| `db` | `postgres:16` | `5432` | PostgreSQL-database `tasks` |
| `pgadmin` | `dpage/pgadmin4` | `5050` | Webgrensesnitt for å inspisere databasen |

## Portvalg

- **8000 i stedet for 80 for Traefik:** Port 80 er ofte opptatt på Windows (IIS, Skype, andre tjenester). For å unngå konflikter lytter Traefik på port 80 *inne i* containeren, men eksponeres på **8000** utenfra.
- **8081 for Traefik-dashbordet:** Traefik bruker 8080 internt for dashbordet. Det mappes til 8081 utenfra.
- **API-et har ingen egen port utenfra:** All trafikk skal gå gjennom Traefik, så API-et eksponeres bevisst ikke direkte. Internt lytter API-et på port 80.
- **Traefik v3.6:** Eldre Traefik-versjoner er ikke kompatible med Docker Engine 29.

## Arkitektur

```
Klient → Traefik (:8000) → API → TaskService → ITaskRepository → EfTaskRepository → PostgreSQL
```

- **Service** er kun avhengig av `ITaskRepository`, ikke av EF Core eller PostgreSQL.
- **`EfTaskRepository`** brukes når API-et kjører, og lagrer i PostgreSQL.
- **`FakeTaskRepository`** brukes fortsatt i unit-testene.

## Kom i gang

Krever **Docker Desktop**.

```powershell
docker compose up --build
```

EF Core-migrationen kjøres automatisk ved oppstart (`Database.Migrate()` i `Program.cs`), så tabellen opprettes uten manuelle steg.

| Hva | Adresse |
|---|---|
| API (via Traefik) | http://localhost:8000/tasks |
| Health check | http://localhost:8000/health |
| Scalar (API-test) | http://localhost:8000/scalar |
| Traefik-dashbord | http://localhost:8081 |
| pgAdmin | http://localhost:5050 |

## pgAdmin

**Innlogging:** `admin@kh.com` / `admin`

**Koble til databasen** (Add New Server → Connection):

| Felt | Verdi |
|---|---|
| Host name/address | `db` |
| Port | `5432` |
| Maintenance database | `tasks` |
| Username | `postgres` |
| Password | `postgres` |

Host er `db` (tjenestenavnet i Compose) og ikke `localhost`, fordi pgAdmin kjører i sin egen container.

Data ligger under **tasks → Schemas → public → Tables → Tasks**.

## Verifisering

1. `POST /tasks` med `{ "title": "Kjøp melk" }` lagrer en oppgave.
2. `GET /tasks` henter den fra PostgreSQL.
3. Oppgaven er synlig i pgAdmin.
4. `docker compose restart api`, så `GET /tasks` igjen: dataene er der fortsatt.
5. `docker ps` viser `api` og `db` som `(healthy)`.

## Tester

Health check-testen sender en forespørsel til `http://localhost:8000/health`, så `docker compose up` må kjøre før testene:

```powershell
dotnet test
```

## Prosjektstruktur

```
├── TaskService.Api
│   ├── Controllers/       TaskController
│   ├── Data/              TaskDbContext
│   ├── DTO/               CreateTaskRequest
│   ├── Repositories/      EfTaskRepository
│   ├── Migrations/        EF Core-migrations
│   ├── Program.cs
│   └── Dockerfile
├── TaskService.Core       TaskService, TaskItem, ITaskRepository, FakeTaskRepository
├── TaskService.test       Unit-tester
└── docker-compose.yaml
```