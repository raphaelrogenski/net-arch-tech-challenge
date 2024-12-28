# Usar uma imagem base do .NET Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Usar uma imagem do SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo .csproj da pasta src/Contact.Api para o diretório de trabalho no container
COPY ["Contact.Api/Contacts.Api.csproj", "Contact.Api/"]

# Restaurar dependências
RUN dotnet restore "Contact.Api/Contacts.Api.csproj"

# Copiar o restante dos arquivos
COPY . .

# Publicar a aplicação
RUN dotnet publish "Contact.Api/Contacts.Api.csproj" -c Release -o /app

# Usar a imagem do runtime para rodar
FROM base AS final
WORKDIR /app

# Copiar os arquivos publicados da etapa de build
COPY --from=build /app .

# Definir o ponto de entrada da aplicação
ENTRYPOINT ["dotnet", "Contacts.Api.dll"]
