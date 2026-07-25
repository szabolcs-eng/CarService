FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["CarServiceApi.csproj", "./"]
RUN dotnet restore "CarServiceApi.csproj"

COPY . .
RUN dotnet publish "CarServiceApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

RUN mkdir -p /app/data

ENTRYPOINT ["dotnet", "CarServiceApi.dll"]