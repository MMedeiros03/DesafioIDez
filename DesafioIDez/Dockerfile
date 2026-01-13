# =========================
# Etapa 1: Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia a solution (.slnx)
COPY ["DesafioIDez.slnx", "./"]

# Copia os csproj (cache eficiente)
COPY ["DesafioIDez/DesafioIDez.Api.csproj", "DesafioIDez/"]
COPY ["DesafioIDez.Aplicacao/DesafioIDez.Aplicacao.csproj", "DesafioIDez.Aplicacao/"]
COPY ["DesafioIDez.Dominio/DesafioIDez.Dominio.csproj", "DesafioIDez.Dominio/"]
COPY ["DesafioIDez.Infraestrutura/DesafioIDez.Infraestrutura.csproj", "DesafioIDez.Infraestrutura/"]

# Restore
RUN dotnet restore "DesafioIDez/DesafioIDez.Api.csproj"

# Copia todo o código
COPY . .

# Build
WORKDIR "/src/DesafioIDez"
RUN dotnet build "DesafioIDez.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build

# =========================
# Etapa 2: Publish
# =========================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "DesafioIDez.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# Etapa 3: Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "DesafioIDez.Api.dll"]
