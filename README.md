# Sample Product API — .NET 8 + Docker + CI/CD

A minimal but production-ready REST API for testing CI/CD pipelines.

## Project Structure

```
SampleApi/
├── SampleApi/                   # ASP.NET Core Web API
│   ├── Controllers/
│   │   └── ProductsController.cs
│   ├── Models/
│   │   └── Product.cs
│   ├── Services/
│   │   ├── IProductService.cs
│   │   └── ProductService.cs
│   └── Program.cs
│
├── SampleApi.Tests/             # xUnit test project
│   ├── Controllers/
│   │   └── ProductsControllerTests.cs   # Moq-based unit tests
│   ├── Services/
│   │   └── ProductServiceTests.cs       # Service unit tests
│   └── Integration/
│       └── ProductsIntegrationTests.cs  # WebApplicationFactory tests
│
├── Dockerfile                   # Multi-stage build
├── docker-compose.yml
└── .github/workflows/ci-cd.yml  # GitHub Actions pipeline
```

## API Endpoints

| Method | Route                | Description        |
|--------|----------------------|--------------------|
| GET    | /api/products        | List all products  |
| GET    | /api/products/{id}   | Get by ID          |
| POST   | /api/products        | Create product     |
| PUT    | /api/products/{id}   | Update product     |
| DELETE | /api/products/{id}   | Delete product     |
| GET    | /health              | Health check       |
| GET    | /swagger             | Swagger UI         |

## Quick Start

### Run locally
```bash
dotnet run --project SampleApi
# → http://localhost:5000/swagger
```

### Run tests locally
```bash
dotnet test
```

### Run with Docker
```bash
# Build & start API
docker compose up --build api

# Run tests inside Docker
docker compose --profile test up test
```

### Build Docker image only
```bash
docker build -t sample-api .
docker run -p 8080:8080 sample-api
```

## CI/CD Pipeline (GitHub Actions)

The `.github/workflows/ci-cd.yml` pipeline runs on every push/PR:

1. **Test** — restore, build, run all tests, collect coverage
2. **Docker** — build & push image to GitHub Container Registry (`main` only)
3. **Deploy** — placeholder step to customize for your target environment

Test results (`.trx`) and coverage (`cobertura XML`) are uploaded as artifacts.
