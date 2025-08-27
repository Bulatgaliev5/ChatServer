# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore ChatServer.csproj
RUN dotnet publish ChatServer.csproj -c Release -o /app/out

# Этап запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS=http://+:${PORT}
ENTRYPOINT ["dotnet", "ChatServer.dll"]
