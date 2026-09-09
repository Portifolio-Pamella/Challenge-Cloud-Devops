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

## Guia de Deploy e Execução na Nuvem (How To)

Este guia descreve os passos exatos para provisionar a infraestrutura e realizar o deploy da aplicação utilizando 100% dos recursos da Microsoft Azure, sem dependências locais.

### 1. Preparação do Ambiente (Azure Cloud Shell)

Todo o processo de orquestração será feito a partir do Azure Cloud Shell para garantir um ambiente padronizado com o Azure CLI já autenticado.

1. Acesse o **Portal da Azure** e faça login.
2. Na barra superior de navegação, clique no ícone **`>_`** para abrir o **Cloud Shell**.
3. Certifique-se de que o terminal está configurado para **Bash** (no canto superior esquerdo da janela do terminal).
4. Clone este repositório para o ambiente do Cloud Shell e entre na pasta dos scripts:

```bash
git clone https://github.com/seu-usuario/Challenge-Cloud-Devops.git
cd Challenge-Cloud-Devops/scripts-azure
chmod +x *.sh

```

### 2. Provisionamento da Infraestrutura Base

Ainda no Cloud Shell, execute os scripts de infraestrutura na seguinte ordem. Aguarde a finalização de cada script antes de iniciar o próximo:

* **Criar a Máquina Virtual (VM) e o Grupo de Recursos:**
```bash
./01_azure_vm_conteiners.sh

```


* **Criar o Azure Container Registry (ACR):**
```bash
./01_5_create_acr.sh

```


* **Criar a Storage Account (Persistência do Banco):**
```bash
./02_storage_account.sh

```


* **Criar o Key Vault e armazenar segredos:**
```bash
./03_key_vault.sh

```



### 3. Build e Push da Imagem (Dentro da VM)

Como o Cloud Shell não possui o motor do Docker instalado, utilizaremos a Máquina Virtual recém-criada (que já possui o Docker instalado pelo script 1) para compilar a imagem da nossa API de forma isolada na nuvem.

* **Descubra o IP Público da VM** executando no Cloud Shell:
```bash
az vm show -d -g rm565206-infra -n rm565206-deploy --query publicIps -o tsv

```


* **Acesse a VM via SSH:**
```bash
ssh admlnx@<COLE_O_IP_AQUI>

```


> **Nota:** A senha é senha padrão utilizada nas aulas `. O terminal não exibirá os caracteres enquanto você digita.


* **Dentro da VM**, assuma as permissões do Docker, clone o projeto e faça o deploy da imagem:
```bash
# Recarrega o usuário para aplicar permissões do Docker
su - admlnx

# Clone o repositório na VM
git clone https://github.com/seu-usuario/Challenge-Cloud-Devops.git
cd Challenge-Cloud-Devops/sistema-veterinario-dotnet

# Faça login no ACR (a senha está no portal Azure > ACR > Chaves de Acesso)
sudo docker login aegisrm565206.azurecr.io -u aegisrm565206

# Crie a imagem da API (Build)
sudo docker build -t aegisrm565206.azurecr.io/rm565206-api:v1 .

# Envie a imagem para o repositório em nuvem (Push)
sudo docker push aegisrm565206.azurecr.io/rm565206-api:v1

```


* **Saia da Máquina Virtual** para retornar ao orquestrador (Cloud Shell):
```bash
exit
exit

```



### 4. Deploy dos Containers (ACI)

Agora você está de volta ao Azure Cloud Shell. Navegue novamente para a pasta de scripts e suba os serviços que consumirão a imagem e o banco de dados.

* **Navegue para a pasta correta:**
```bash
cd ~/Challenge-Cloud-Devops/scripts-azure

```


* **Suba o Banco de Dados (Oracle) com persistência:**
```bash
./04_deploy_oracle_aci.sh

```


* **Suba a API (.NET) conectada ao banco e ao Key Vault:**
```bash
./05_deploy_api_aci.sh

```



### 5. Validação e Testes

Para confirmar que tudo está funcionando e realizar as operações CRUD exigidas:

1. **Testes na API:**
* No portal da Azure, vá em **Container Instances** e clique no recurso `api-dotnet`.
* Copie o **Endereço IP / FQDN** público fornecido na visão geral.
* Acesse `http://<IP_DA_API>:8080/swagger`.
* Teste a inserção de dados utilizando os modelos que estão disponíveis no arquivo `testes.json` na pasta `docs` do nosso repositório.


2. **Testes no Banco de Dados (Oracle):**
* Acesse o terminal do banco de dados executando o seguinte comando no Cloud Shell:
```bash
az container exec --resource-group rg-aegis-app --name oracle-dimdim --exec-command "sqlplus system/200806@//localhost:1521/XE"

```


* Dentro do banco, faça o `SELECT` para validar os dados recém-inseridos via API:
```sql
SELECT * FROM TB_VETERINARIO;
SELECT * FROM TB_PET;

```