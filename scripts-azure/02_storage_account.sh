#!/bin/bash
rm="565206"
RESOURCE_GROUP="rg-aegis-app"
LOCATION="canadacentral"
STORAGE_ACCOUNT="volumeaegisdata$rm"
FILE_SHARE="oracle-aegis-volume"

az provider register --namespace Microsoft.Storage

if ! az storage account show --name "$STORAGE_ACCOUNT" --resource-group "$RESOURCE_GROUP" &>/dev/null; then
  az group create --name "$RESOURCE_GROUP" --location "$LOCATION"
  az storage account create --resource-group "$RESOURCE_GROUP" \
    --name "$STORAGE_ACCOUNT" \
    --location "$LOCATION" \
    --sku Standard_LRS
else
  echo "A conta de armazenamento '$STORAGE_ACCOUNT' já existe"
fi

connection_string=$(az storage account show-connection-string --name "$STORAGE_ACCOUNT" --resource-group "$RESOURCE_GROUP" --query connectionString --output tsv)

if ! az storage share exists --name "$FILE_SHARE" --account-name "$STORAGE_ACCOUNT" --connection-string "$connection_string" | grep -q true; then
  az storage share create --name "$FILE_SHARE" --account-name "$STORAGE_ACCOUNT" --connection-string "$connection_string"
else
  echo "O compartilhamento de arquivos '$FILE_SHARE' já existe"
fi