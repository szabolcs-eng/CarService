# 1. Építési fázis (Build)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Projektfájl másolása és a NuGet csomagok letöltése
COPY ["CarServiceApi.csproj", "./"]
RUN dotnet restore "CarServiceApi.csproj"

# A kód többi részének másolása és a Release build elkészítése
COPY . .
RUN dotnet publish "CarServiceApi.csproj" -c Release -o /app/publish

# 2. Futtatási fázis (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Létrehozzuk a mappát az SQLite adatbázisnak a konténeren belül
RUN mkdir -p /app/data

ENTRYPOINT ["dotnet", "CarServiceApi.dll"]