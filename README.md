# Note App (Frontend + Backend)

## Structure
- frontend (Vue)
- backend (ASP.NET Core)

---

## 🚀 Run Frontend

```bash
cd frontend
cp .env.example .env
npm install
npm run dev
```

---

## 🚀 Run Backend

```bash
cd backend
dotnet restore
```

Override settings via environment variables (`__` separates hierarchy levels):

```bash
ConnectionStrings__DefaultConnection="Server=..." \
Jwt__Key="your-secret-key-at-least-32-chars" \
Jwt__Audience="note-app" \
dotnet run
```

---

## 🔗 Env Setup

Frontend `.env`:
```
VITE_API_BASE_URL=http://localhost:5000
```

Backend env vars (overrides `appsettings.json`):

| Variable | Description |
|---|---|
| `ConnectionStrings__DefaultConnection` | SQL Server connection string |
| `Jwt__Key` | JWT signing key (min 32 chars) |
| `Jwt__Issuer` | JWT issuer (default: `NoteAppApi`) |
| `Jwt__Audience` | JWT audience |
| `Jwt__ExpireMinutes` | Token expiry in minutes (default: `60`) |

---

## 🐳 Run with Docker Compose

```bash
docker compose up --build
```

- Frontend: http://localhost:3000
- Backend API: http://localhost:5001
- Swagger: http://localhost:5001/swagger

Three containers: `frontend`, `backend`, `sqlserver`.

---