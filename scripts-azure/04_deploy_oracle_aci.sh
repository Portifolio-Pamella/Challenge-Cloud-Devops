
#!/bin/bash
rm="565206"
resourceGroup="rg-aegis-app"
aciName="oracle-dimdim"
keyVaultName="keyvault-$rm"

echo "Subindo o Container do Banco de Dados Oracle..."

az container create \
  --resource-group "$resourceGroup" \
  --name "$aciName" \
  --image "container-registry.oracle.com/database/express:latest" \
  --ip-address Public \
  --dns-name-label "oracle-container-$rm" \
  --cpu 2 \
  --memory 4 \
  --os-type Linux \
  --ports 1521 \
  --environment-variables \
    ORACLE_PWD="$(az keyvault secret show --vault-name "$keyVaultName" --name "oracle-password" --query value -o tsv)" \
  --restart-policy Always
