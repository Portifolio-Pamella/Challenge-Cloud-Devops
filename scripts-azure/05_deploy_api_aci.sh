#!/bin/bash

# Interrompe o script se houver qualquer erro
set -e

echo "===================================================="
echo "Iniciando o Deploy da Arquitetura (Oracle DB + .NET API)"
echo "===================================================="

RG_NAME="rg-aegis-app"
ACR_NAME="aegisrm565206"
DB_IMAGE="${ACR_NAME}.azurecr.io/rm565206-db:v1"
API_IMAGE="${ACR_NAME}.azurecr.io/rm565206-api:v1"

# Senha padrão que será criada para o usuário SYSTEM no Oracle
ORACLE_PASS="Fiap@2tdsvms2026" 

echo "1. Obtendo credenciais do cofre ACR..."
ACR_PASS=$(az acr credential show -n $ACR_NAME --query "passwords[0].value" -o tsv)

echo "2. Subindo o Container do Banco de Dados (Isso leva alguns minutos)..."
az container create \
  --resource-group $RG_NAME \
  --name aci-oracle-db \
  --image $DB_IMAGE \
  --os-type Linux \
  --registry-login-server ${ACR_NAME}.azurecr.io \
  --registry-username $ACR_NAME \
  --registry-password $ACR_PASS \
  --dns-name-label db-aegis565206 \
  --ports 1521 \
  --cpu 2 --memory 4 \
  --environment-variables ORACLE_PASSWORD=$ORACLE_PASS

echo "3. Capturando a URL do Banco de Dados..."
DB_FQDN=$(az container show -g $RG_NAME -n aci-oracle-db --query ipAddress.fqdn -o tsv)
echo "Banco no ar em: $DB_FQDN"

# Monta a Connection String apontando para o Oracle interno
CONN_STRING="Data Source=$DB_FQDN:1521/FREEPDB1;User Id=system;Password=$ORACLE_PASS;"

echo "4. Subindo o Container da API e conectando ao Banco..."
az container create \
  --resource-group $RG_NAME \
  --name aci-dotnet-api \
  --image $API_IMAGE \
  --os-type Linux \
  --registry-login-server ${ACR_NAME}.azurecr.io \
  --registry-username $ACR_NAME \
  --registry-password $ACR_PASS \
  --dns-name-label api-aegis565206 \
  --ports 8080 \
  --cpu 1 --memory 2 \
  --environment-variables ConnectionStrings__OracleConnection="$CONN_STRING"

echo "===================================================="
API_FQDN=$(az container show -g $RG_NAME -n aci-dotnet-api --query ipAddress.fqdn -o tsv)
echo "Deploy finalizado com sucesso! 🎉"
echo "Sua API está disponível no navegador em: http://$API_FQDN:8080"
echo "===================================================="