# Sistema Veterinário - Aegis | DevOps Tools & Cloud Computing

**Grupo:**

* Felipe Ribeiro Salles de Camargo | RM565224
* João Pedro Pereira Camilo | RM562005
* Lucas Matsubara Reis | RM565020
* Pamella Christiny Chaves Brito | RM565206

---

## 1. Descrição da Solução

O **Sistema Veterinário Aegis** é uma plataforma moderna desenvolvida em arquitetura limpa (Clean Architecture) com **.NET 8.0 na API** e **Oracle Database** para persistência de dados. A solução foi projetada para otimizar a gestão de clínicas e hospitais veterinários, centralizando o controle de cadastros de pacientes, tutores, prontuários médicos, agendamentos de consultas e histórico de tratamentos.

Para garantir alta disponibilidade, escalabilidade e conformidade com as melhores práticas de engenharia de software e computação em nuvem, a aplicação adota uma abordagem conteinerizada utilizando **Docker**, orquestrada e hospedada nativamente na **Microsoft Azure** com serviços gerenciados.

---

## 2. Descrição dos Benefícios para o Negócio

A adoção desta arquitetura baseada em nuvem traz impactos diretos e mensuráveis para a operação do negócio veterinário e para a maturidade técnica da engenharia:

* **Resiliência e Escalabilidade Dinâmica:** Com o uso do **Azure Container Instances (ACI)** e do **Azure Container Registry (ACR)**, a API pode ser escalada instantaneamente conforme a demanda de acessos dos tutores e veterinários, sem a necessidade de gerenciar servidores físicos dedicados.
* **Isolamento de Carga Crítica:** O banco de dados Oracle XE possui demandas específicas e robustas de hardware. Isolá-lo em uma instância de container dedicada com volume persistente (**Azure Files / Storage Account**) garante que picos de processamento na API não afetem a integridade ou a performance das transações financeiras e médicas.
* **Segurança da Informação e Conformidade (Non-Root):** A aplicação .NET foi empacotada em uma imagem Docker utilizando uma estratégia multi-estágio que executa o processo sob um usuário restrito (**non-root** / usuário `app`). Isso mitiga vetores de invasão por escalação de privilégios no host, protegendo dados sensíveis de animais e tutores em conformidade com a LGPD.
* **Gestão Centralizada de Segredos:** A utilização do **Azure Key Vault** elimina o armazenamento de credenciais, strings de conexão e senhas de banco de dados em texto plano no código-fonte ou em arquivos de configuração locais.
* **Agilidade no Deploy (CI/CD Ready):** A automação via Azure CLI padroniza os ambientes de homologação e produção, reduzindo a zero o risco de falhas humanas durante o provisionamento de infraestrutura.

---

## 3. Arquitetura da Solução e Fluxo de Funcionamento

Abaixo está o desenho da arquitetura proposta para o Sistema Veterinário Aegis, demonstrando o fluxo de implantação via Azure CLI e a comunicação entre os recursos na nuvem.

![Arquitetura da Solução Aegis](docs/ArquiteturaSprint3.jpg)

### Explicação do Fluxo:
1. **Ambiente de Gestão (VM):** Uma máquina virtual (`rm565206-deploy`) provisionada isoladamente executa os scripts da Azure CLI (`.sh`) para orquestrar toda a infraestrutura como código (IaC).
2. **Registro de Imagens:** O código fonte da API .NET é compilado (build) sem privilégios de root e enviado (push) para o **Azure Container Registry (ACR)** (`aegisrm565206`).
3. **Gestão de Segredos:** O **Azure Key Vault** armazena credenciais do ACR, senhas do Oracle e strings de conexão, injetando-as nos containers em tempo de execução de forma segura.
4. **Persistência do Banco:** O banco de dados **Oracle XE** roda em um **Azure Container Instances (ACI)** (`oracle-dimdim`). Para garantir a persistência dos dados (operações CRUD no sistema veterinário), um volume é montado apontando para um File Share no **Azure Storage Account**.
5. **Aplicação:** A API .NET é instanciada em outro ACI (`api-dotnet`), comunicando-se de forma segura com o banco de dados utilizando as credenciais fornecidas pelo Key Vault, e expõe a porta 8080 para consumo via Internet.

```

### Explicação do Fluxo:

1. **Provisionamento Automatizado:** Todos os recursos são estritamente gerados por scripts acoplados à **Azure CLI**, garantindo rastreabilidade e infraestrutura como código (IaC).
2. **Registro de Imagens:** O código fonte da API .NET é compilado e enviado para o **ACR (`aegisrm565206`)**.
3. **Persistência do Banco:** O banco de dados **Oracle XE** roda em um ACI dedicado (`oracle-dimdim`), mapeando seu diretório de dados (`/opt/oracle/oradata`) diretamente para um File Share em nuvem (**Storage Account**). Mesmo se o container cair, os dados do sistema veterinário permanecem intactos.
4. **Segurança de Conexão:** A API .NET é instanciada em outro ACI (`api-dotnet`) configurado para buscar dinamicamente as strings de acesso e credenciais protegidas no **Azure Key Vault**.

---

## 4. Pré-requisitos e Configuração Inicial

* Conta ativa na Microsoft Azure com permissões para criar Grupos de Recursos.
* [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) instalado localmente ou utilização direta do **Azure Cloud Shell**.
* Git instalado para clonar o repositório.

**Clone obrigatório do repositório para iniciar os testes:**

```bash
git clone https://github.com/seu-usuario/sistema-veterinario-dotnet.git
cd sistema-veterinario-dotnet

```

---

## 5. Guia de Deploy e Execução na Nuvem (Passo a Passo)

### Passo 1: Criação da Infraestrutura da VM de Apoio / Gestão

```bash
#!/bin/bash
RESOURCE_GROUP="rm565206-infra"
VM_NAME="rm565206-deploy"
LOCATION="canadacentral"
IMAGE="almalinux:almalinux-x86_64:10-gen2:10.1.202512150"
SIZE="Standard_B2ats_v2"
USERNAME="admlnx"
PASSWORD="Fiap@2tdsvms"

az group create --name "$RESOURCE_GROUP" --location "$LOCATION"

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

az network nsg rule create \
    --resource-group "$RESOURCE_GROUP" \
    --nsg-name nsg-linux-free \
    --name Common_Ports \
    --protocol tcp \
    --priority 1111 \
    --destination-port-ranges 80 8080 3000 5000 5001 \
    --access allow \
    --source-address-prefixes "*"

```

### Passo 2: Criação do Azure Container Registry (ACR)

```bash
#!/bin/bash
rm="565206"
RESOURCE_GROUP="rg-aegis-app"
LOCATION="canadacentral"
ACR_NAME="aegisrm$rm"

az group create --name "$RESOURCE_GROUP" --location "$LOCATION"

az acr create \
    --resource-group "$RESOURCE_GROUP" \
    --name "$ACR_NAME" \
    --sku Basic \
    --admin-enabled true

```

### Passo 3: Configuração da Storage Account para Persistência do Oracle

```bash
#!/bin/bash
rm="565206"
RESOURCE_GROUP="rg-aegis-app"
LOCATION="canadacentral"
STORAGE_ACCOUNT="volumeaegisdata$rm"
FILE_SHARE="oracle-aegis-volume"

az provider register --namespace Microsoft.Storage

if ! az storage account show --name "$STORAGE_ACCOUNT" --resource-group "$RESOURCE_GROUP" &>/dev/null; then
  az storage account create --resource-group "$RESOURCE_GROUP" \
    --name "$STORAGE_ACCOUNT" \
    --location "$LOCATION" \
    --sku Standard_LRS
fi

connection_string=$(az storage account show-connection-string --name "$STORAGE_ACCOUNT" --resource-group "$RESOURCE_GROUP" --query connectionString --output tsv)

if ! az storage share exists --name "$FILE_SHARE" --account-name "$STORAGE_ACCOUNT" --connection-string "$connection_string" | grep -q true; then
  az storage share create --name "$FILE_SHARE" --account-name "$STORAGE_ACCOUNT" --connection-string "$connection_string"
fi

```

### Passo 4: Configuração do Key Vault

```bash
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
fi

az role assignment create \
  --assignee "$(az account show --query user.name -o tsv)" \
  --role "Key Vault Administrator" \
  --scope "/subscriptions/$(az account show --query id -o tsv)/resourceGroups/$resourceGroup/providers/Microsoft.KeyVault/vaults/$keyVaultName"

sleep 15

ACRUSERNAME=$(az acr credential show --name "$acrName" --resource-group "$resourceGroup" --query username --output tsv)
ACRPASSWORD=$(az acr credential show --name "$acrName" --resource-group "$resourceGroup" --query passwords[0].value --output tsv)

az keyvault secret set --vault-name "$keyVaultName" --name "oracle-password" --value "$ORACLE_PASSWORD"
az keyvault secret set --vault-name "$keyVaultName" --name "connection-strings" --value "$CONNECTIONSTRINGS"
az keyvault secret set --vault-name "$keyVaultName" --name "acr-username" --value "$ACRUSERNAME"
az keyvault secret set --vault-name "$keyVaultName" --name "acr-password" --value "$ACRPASSWORD"

```

### Passo 5: Build e Push da Imagem .NET para o ACR

Na raiz do projeto onde se encontra o `Dockerfile`:

```bash
az acr login --name aegisrm565206

docker build -t aegisrm565206.azurecr.io/rm565206-api:v1 .

docker push aegisrm565206.azurecr.io/rm565206-api:v1

```

### Passo 6: Deploy do Banco Oracle no ACI (Com Persistência)

```bash
#!/bin/bash
rm="565206"
resourceGroup="rg-aegis-app"
acrName="aegisrm$rm"
aciName="oracle-dimdim"
storageAccountName="volumeaegisdata$rm"
file_share_name="oracle-aegis-volume"
storage_key=$(az storage account keys list --resource-group "$resourceGroup" --account-name "$storageAccountName" --query "[0].value" --output tsv)
keyVaultName="keyvault-aegis-$rm"

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

```

### Passo 7: Deploy da Aplicação .NET no ACI (Non-Root)

```bash
#!/bin/bash
rm="565206"
resourceGroup="rg-aegis-app"
acrName="aegisrm$rm"
aciName="api-dotnet"
aciNameOracle="oracle-dimdim"
imageName="rm565206-api"
tag="v1"
keyVaultName="keyvault-aegis-$rm"

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

```

---

## 6. Dockerfile Seguro (Non-Root)

O arquivo `Dockerfile` abaixo emprega compilação multi-estágio e assegura o cumprimento da regra de segurança, rodando sob um usuário não privilegiado (`app`):

```dockerfile
# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SistemaVeterinario.API/SistemaVeterinario.API.csproj", "SistemaVeterinario.API/"]
RUN dotnet restore "SistemaVeterinario.API/SistemaVeterinario.API.csproj"
COPY . .
WORKDIR "/src/SistemaVeterinario.API"
RUN dotnet publish -c Release -o /app/publish

# Estágio de Runtime (Execução Segura)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# REQUISITO DE SEGURANÇA: Executa a aplicação como non-root (usuário padrão 'app' do ASP.NET)
USER app

EXPOSE 8080
ENTRYPOINT ["dotnet", "SistemaVeterinario.API.dll"]

```

---

## 7. Banco de Dados: DDL das Tabelas (Core da Aplicação)

Abaixo estão os scripts DDL referentes às tabelas principais do sistema veterinário (`T_VET_TUTOR` e `T_VET_PET`), criadas para suportar o CRUD completo com persistência garantida.

```sql
-- DDL da Tabela de Tutores (Core 1)
CREATE TABLE T_VET_TUTOR (
    id_tutor NUMBER GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    nm_tutor VARCHAR2(100) NOT NULL,
    ds_email VARCHAR2(100) UNIQUE NOT NULL,
    nr_telefone VARCHAR2(20) NOT NULL,
    dt_cadastro DATE DEFAULT SYSDATE
);

-- DDL da Tabela de Pets (Core 2)
CREATE TABLE T_VET_PET (
    id_pet NUMBER GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    id_tutor NUMBER NOT NULL,
    nm_pet VARCHAR2(50) NOT NULL,
    ds_especie VARCHAR2(50) NOT NULL,
    ds_raca VARCHAR2(50),
    nr_idade NUMBER(3),
    CONSTRAINT fk_tutor_pet FOREIGN KEY (id_tutor) REFERENCES T_VET_TUTOR(id_tutor)
);

```

### Inserção de Dados Significativos (Manipulação de Linhas)

```sql
-- Inserção de Tutores
INSERT INTO T_VET_TUTOR (nm_tutor, ds_email, nr_telefone) 
VALUES ('Carlos Eduardo Silva', 'carlos.silva@email.com', '(11) 98765-4321');

INSERT INTO T_VET_TUTOR (nm_tutor, ds_email, nr_telefone) 
VALUES ('Mariana Souza Lima', 'mariana.lima@email.com', '(11) 91234-5678');

-- Inserção de Pets vinculados aos Tutores
INSERT INTO T_VET_PET (id_tutor, nm_pet, ds_especie, ds_raca, nr_idade) 
VALUES (1, 'Mel', 'Canino', 'Golden Retriever', 3);

INSERT INTO T_VET_PET (id_tutor, nm_pet, ds_especie, ds_raca, nr_idade) 
VALUES (2, 'Thor', 'Felino', 'Persa', 2);

COMMIT;

```