# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.sln .
COPY NetArchTechChallenge.Gateway/*.csproj NetArchTechChallenge.Gateway/
RUN dotnet restore NetArchTechChallenge.Gateway/NetArchTechChallenge.Gateway.csproj
COPY . .
WORKDIR /src/NetArchTechChallenge.Gateway
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8000
ENTRYPOINT ["dotnet", "NetArchTechChallenge.Gateway.dll"]