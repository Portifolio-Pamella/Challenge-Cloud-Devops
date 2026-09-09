# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Aegis.Api.csproj", "./"]
RUN dotnet restore "Aegis.Api.csproj"
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Estágio de Runtime (Execução)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# REQUISITO DE SEGURANÇA: Executa a aplicação como non-root (usuário padrão 'app' do ASP.NET)
USER app
EXPOSE 8080
ENTRYPOINT ["dotnet", "Aegis.Api.dll"]