#!/bin/bash
RESOURCE_GROUP="rm565206-infra"
VM_NAME="rm565206-deploy"
LOCATION="canadacentral"
IMAGE="almalinux:almalinux-x86_64:10-gen2:10.1.202512150"
SIZE="Standard_B2ats_v2"
USERNAME="admlnx"
PASSWORD="Fiap@2tdsvms"

echo "Criando Resource Group: $RESOURCE_GROUP"
az group create --name "$RESOURCE_GROUP" --location "$LOCATION"

echo "Criando VM e infraestrutura de rede..."
az vm create \
    --resource-group "$RESOURCE_GROUP" \
    --name "$VM_NAME" \
    --location "$LOCATION" \
    --image "$IMAGE" \
    --size "$SIZE" \
    --admin-username "$USERNAME" \
    --admin-password "$PASSWORD" \
    --vnet-name vnet-linux-free \
    --vnet-address-prefix 10.0.0.0/16 \
    --subnet subnet-linux-free \
    --subnet-address-prefix 10.0.1.0/24 \
    --nsg nsg-linux-free \
    --nsg-rule SSH \
    --public-ip-sku Standard \
    --public-ip-address ip-linux-free \
    --nic-delete-option Delete \
    --os-disk-delete-option Delete \
    --storage-sku Premium_LRS \
    --os-disk-size-gb 64

echo "Abrindo portas comuns no Firewall..."
az network nsg rule create \
    --resource-group "$RESOURCE_GROUP" \
    --nsg-name nsg-linux-free \
    --name Common_Ports \
    --protocol tcp \
    --priority 1111 \
    --destination-port-ranges 80 8080 3000 5000 5001 \
    --access allow \
    --source-address-prefixes "*"