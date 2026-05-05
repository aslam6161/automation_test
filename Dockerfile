# ─────────────────────────────────────────────────────────────────────────────
# Stage 1 – Build & Test
# ─────────────────────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first (layer-cache friendly)
COPY SampleApi.sln .
COPY SampleApi/SampleApi.csproj             SampleApi/
COPY SampleApi.Tests/SampleApi.Tests.csproj SampleApi.Tests/

# Restore NuGet packages
RUN dotnet restore

# Copy the rest of the source
COPY . .

# Build the solution
RUN dotnet build --no-restore -c Release

# ─────────────────────────────────────────────────────────────────────────────
# Stage 2 – Run Tests  (separate layer so CI can inspect test results)
# ─────────────────────────────────────────────────────────────────────────────
FROM build AS test
WORKDIR /src
RUN dotnet test --no-build -c Release \
    --logger "trx;LogFileName=test-results.trx" \
    --results-directory /TestResults \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=cobertura \
    /p:CoverletOutput=/TestResults/coverage.xml

# ─────────────────────────────────────────────────────────────────────────────
# Stage 3 – Publish
# ─────────────────────────────────────────────────────────────────────────────
FROM build AS publish
WORKDIR /src/SampleApi
RUN dotnet publish -c Release -o /app/publish --no-build

# ─────────────────────────────────────────────────────────────────────────────
# Stage 4 – Runtime (lean image)
# ─────────────────────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Non-root user for security
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
USER appuser

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "SampleApi.dll"]
