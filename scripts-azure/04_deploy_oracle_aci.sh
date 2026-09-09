#!/bin/bash
rm="565206"
resourceGroup="rg-aegis-app"
acrName="aegisrm$rm"
aciName="oracle-dimdim"
storageAccountName="volumeaegisdata$rm"
file_share_name="oracle-aegis-volume"
storage_key=$(az storage account keys list --resource-group "$resourceGroup" --account-name "$storageAccountName" --query "[0].value" --output tsv)
keyVaultName="keyvault-$rm"

az provider register --namespace Microsoft.ContainerInstance

az container create \
  --resource-group "$resourceGroup" \
  --name "$aciName" \
  --image "container-registry.oracle.com/database/express:latest" \
  --ip-address Public \
  --cpu 1 \
  --memory 2 \
  --os-type Linux \
  --dns-name-label "oracle-container-$rm" \
  --ports 1521 \
  --azure-file-volume-account-name "$storageAccountName" \
  --azure-file-volume-account-key "$storage_key" \
  --azure-file-volume-share-name "$file_share_name" \
  --azure-file-volume-mount-path "/opt/oracle/oradata" \
  --environment-variables \
    ORACLE_PWD="$(az keyvault secret show --vault-name "$keyVaultName" --name "oracle-password" --query value -o tsv)" \
  --restart-policy Always