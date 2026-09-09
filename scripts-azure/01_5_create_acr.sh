#!/bin/bash
rm="565206"
RESOURCE_GROUP="rg-aegis-app"
LOCATION="canadacentral"
ACR_NAME="aegisrm$rm"

echo "Criando Grupo de Recursos da Aplicação: $RESOURCE_GROUP"
az group create --name "$RESOURCE_GROUP" --location "$LOCATION"

echo "Criando Azure Container Registry (ACR): $ACR_NAME"
az acr create \
    --resource-group "$RESOURCE_GROUP" \
    --name "$ACR_NAME" \
    --sku Basic \
    --admin-enabled true