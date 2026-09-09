# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copia absolutamente todas as pastas do projeto (API, Domain, Infrastructure, etc)
COPY . .

# 2. Restaura as dependências apontando para dentro da subpasta da API
RUN dotnet restore "SistemaVeterinario.API/SistemaVeterinario.API.csproj"

# 3. Compila e publica o projeto apontando para a mesma subpasta
RUN dotnet publish "SistemaVeterinario.API/SistemaVeterinario.API.csproj" -c Release -o /app/publish

# Estágio de Runtime (Execução)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# REQUISITO DE SEGURANÇA: Executa a aplicação como non-root (usuário padrão 'app' do ASP.NET)
USER app
EXPOSE 8080
ENTRYPOINT ["dotnet", "SistemaVeterinario.API.dll"]