# Usar uma imagem base do .NET Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Usar uma imagem do SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo .csproj e restaurar as dependências
COPY ["Contact.Api/Contacts.Api.csproj", "Contact.Api/"]
RUN dotnet restore "Contact.Api/Contacts.Api.csproj"

# Copiar o restante dos arquivos e compilar
COPY . .
WORKDIR "/src/Contact.Api"
RUN dotnet publish -c Release -o /app

# Usar a imagem do runtime para rodar
FROM base AS final
WORKDIR /app

# Copiar os arquivos publicados
COPY --from=build /app .

# Expor a porta para a API
EXPOSE 8080

# Definir o ponto de entrada
ENTRYPOINT ["dotnet", "Contacts.Api.dll"]
