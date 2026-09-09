#!/bin/bash
rm="565206"
resourceGroup="rg-aegis-app"
acrName="aegisrm$rm"
aciName="api-dotnet"
aciNameOracle="oracle-dimdim"
imageName="rm565206-api"
tag="v1"
keyVaultName="keyvault-$rm"

oraclePublicIP=$(az container show --resource-group "$resourceGroup" --name "$aciNameOracle" --query ipAddress.ip --output tsv)

az provider register --namespace Microsoft.ContainerInstance

az container create \
  --resource-group "$resourceGroup" \
  --name "$aciName" \
  --image "$acrName.azurecr.io/$imageName:$tag" \
  --cpu 1 \
  --memory 1.5 \
  --os-type Linux \
  --dns-name-label "api-dotnet-container-$rm" \
  --ports 8080 \
  --registry-login-server "$acrName.azurecr.io" \
  --registry-username "$(az keyvault secret show --vault-name "$keyVaultName" --name "acr-username" --query value -o tsv)" \
  --registry-password "$(az keyvault secret show --vault-name "$keyVaultName" --name "acr-password" --query value -o tsv)" \
  --environment-variables \
    ConnectionStrings__DefaultConnection="$(az keyvault secret show --name "connection-strings" --vault-name "$keyVaultName" --query value -o tsv | sed "s/oracle-dimdim/$oraclePublicIP/")" \
    ASPNETCORE_ENVIRONMENT="Development" \
    ASPNETCORE_URLS="http://+:8080" \
  --restart-policy Always