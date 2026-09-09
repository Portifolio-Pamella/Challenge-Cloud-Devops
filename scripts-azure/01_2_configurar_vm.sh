#!/bin/bash

# ==============================================================================
# SCRIPT 01_5: Configuração do Ambiente Interno da VM (Docker, Git e Ferramentas)
# Onde executar: No Azure Cloud Shell logo após o script 01
# ==============================================================================

RESOURCE_GROUP="rm565206-infra"
VM_NAME="rm565206-deploy"

echo "==> Iniciando a preparação e instalação de pacotes na VM ($VM_NAME)..."

az vm run-command invoke \
  --resource-group "$RESOURCE_GROUP" \
  --name "$VM_NAME" \
  --command-id RunShellScript \
  --scripts "
    echo '1. Atualizando pacotes do sistema...'
    sudo dnf update -y

    echo '2. Instalando Git, Curl e utilitários...'
    sudo dnf install -y git curl wget yum-utils

    echo '3. Configurando repositório e instalando o Docker...'
    sudo dnf config-manager --add-repo https://download.docker.com/linux/centos/docker-ce.repo
    sudo dnf install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

    echo '4. Iniciando e habilitando o serviço do Docker...'
    sudo systemctl start docker
    sudo systemctl enable docker

    echo '5. Adicionando o usuário ao grupo do Docker...'
    sudo usermod -aG docker \$USER
  "

echo "=================================================="
echo " AMBIENTE DA VM CONFIGURADO COM SUCESSO! "
echo "=================================================="