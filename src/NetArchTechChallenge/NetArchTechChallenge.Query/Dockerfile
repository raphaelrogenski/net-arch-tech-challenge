# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.sln ./
COPY NetArchTechChallenge.Query/*.csproj NetArchTechChallenge.Query/
COPY NetArchTechChallenge.Shared.*/*.csproj ./
RUN dotnet restore NetArchTechChallenge.Query/NetArchTechChallenge.Query.csproj
COPY . ./
WORKDIR /src/NetArchTechChallenge.Query
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0
WORKDIR /home/site/wwwroot
COPY --from=build /app/publish ./