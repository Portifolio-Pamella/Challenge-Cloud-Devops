#!/bin/bash
# Script: 03_5_build_e_push_vm.sh

RESOURCE_GROUP="rm565206-infra"
VM_NAME="rm565206-deploy"
ACR_NAME="aegisrm565206"
RG_APP="rg-aegis-app"
# IMPORTANTE: Troque para a URL real do seu repositório
REPO_URL="https://github.com/seu-usuario/Challenge-Cloud-Devops.git" 

echo "==> 1. Buscando senha segura do Azure Container Registry..."
ACR_PASSWORD=$(az acr credential show --name $ACR_NAME --resource-group $RG_APP --query passwords[0].value --output tsv)

echo "==> 2. Enviando comandos para a VM executar o Build e Push (Isso pode demorar alguns minutos)..."
az vm run-command invoke \
  --resource-group "$RESOURCE_GROUP" \
  --name "$VM_NAME" \
  --command-id RunShellScript \
  --scripts "
    echo 'Instalando Docker e Git...'
    sudo dnf install -y docker git
    sudo systemctl start docker
    sudo systemctl enable docker
    
    echo 'Clonando repositório temporariamente...'
    rm -rf repo-temp
    git clone $REPO_URL repo-temp
    cd repo-temp/sistema-veterinario-dotnet
    
    echo 'Autenticando no ACR e gerando Imagem...'
    sudo docker login $ACR_NAME.azurecr.io -u $ACR_NAME -p $ACR_PASSWORD
    sudo docker build -t $ACR_NAME.azurecr.io/rm565206-api:v1 .
    sudo docker push $ACR_NAME.azurecr.io/rm565206-api:v1
    
    echo 'Limpando arquivos da VM...'
    cd ../..
    rm -rf repo-temp
  "

echo "======================================================"
echo "Build e Push concluídos com sucesso 100% via Nuvem!"
echo "======================================================"