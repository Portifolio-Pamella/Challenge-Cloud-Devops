#!/bin/bash
rm="565206"
resourceGroup="rg-aegis-app"
location="canadacentral"

ORACLE_PASSWORD="200806"
CONNECTIONSTRINGS='Data Source=oracle-dimdim:1521/XE;User Id=rm565206;Password=200806;'

acrName="aegisrm$rm"
keyVaultName="keyvault-aegis-$rm"

az provider register --namespace Microsoft.KeyVault

if ! az keyvault show --name "$keyVaultName" --resource-group "$resourceGroup" &> /dev/null; then
  az keyvault create --name "$keyVaultName" --resource-group "$resourceGroup" --location "$location"
else
  echo "Key Vault '$keyVaultName' já existe."
fi

echo "Atribuindo permissões de acesso ao Key Vault..."
az role assignment create \
  --assignee "$(az account show --query user.name -o tsv)" \
  --role "Key Vault Administrator" \
  --scope "/subscriptions/$(az account show --query id -o tsv)/resourceGroups/$resourceGroup/providers/Microsoft.KeyVault/vaults/$keyVaultName"

echo "Aguardando propagação de permissões..."
sleep 15

ACRUSERNAME=$(az acr credential show --name "$acrName" --resource-group "$resourceGroup" --query username --output tsv)
ACRPASSWORD=$(az acr credential show --name "$acrName" --resource-group "$resourceGroup" --query passwords[0].value --output tsv)

az keyvault secret set --vault-name "$keyVaultName" --name "oracle-password" --value "$ORACLE_PASSWORD"
az keyvault secret set --vault-name "$keyVaultName" --name "connection-strings" --value "$CONNECTIONSTRINGS"
az keyvault secret set --vault-name "$keyVaultName" --name "acr-username" --value "$ACRUSERNAME"
az keyvault secret set --vault-name "$keyVaultName" --name "acr-password" --value "$ACRPASSWORD"