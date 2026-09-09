A adição desse script foi uma excelente sacada! O arquivo `03_5_build_e_push_vm.sh` usa o recurso **Azure Run Command**, o que significa que ele injeta os comandos direto na Máquina Virtual pela nuvem.

Com isso, **você não precisa mais daquela etapa manual de acessar a VM via SSH**, digitar a senha, clonar e fazer o build na mão. O script faz tudo isso sozinho de forma automatizada.

Abaixo está o seu `README.md` completamente atualizado. Aproveitei para substituir os links genéricos (`seu-usuario`) pelo link correto do seu repositório (`Portifolio-Pamella`), assim quem for testar o seu projeto só precisa copiar e colar.

Aqui está o código completo para você substituir no seu arquivo `README.md`:

```markdown
# Sistema Veterinário | DevOps Tools & Cloud Computing

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
git clone [https://github.com/Portifolio-Pamella/Challenge-Cloud-Devops.git](https://github.com/Portifolio-Pamella/Challenge-Cloud-Devops.git)
cd Challenge-Cloud-Devops/sistema-veterinario-dotnet

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
git clone [https://github.com/Portifolio-Pamella/Challenge-Cloud-Devops.git](https://github.com/Portifolio-Pamella/Challenge-Cloud-Devops.git)
cd Challenge-Cloud-Devops/scripts-azure
chmod +x *.sh

```

### 2. Provisionamento da Infraestrutura Base

Ainda no Cloud Shell, execute os scripts de infraestrutura na seguinte ordem. Aguarde a finalização de cada script antes de iniciar o próximo:

* **Criar a Máquina Virtual (VM) e o Grupo de Recursos:**

```bash
./01_azure_vm_conteiners.sh

```

* **Configurar dependências na VM:**

```bash
./01_2_configurar_vm.sh

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

### 3. Build e Push da Imagem Automatizado (Via Nuvem)

Graças à automação utilizando o `Run Command`, não é necessário acessar a Máquina Virtual manualmente via SSH. O script a seguir envia as instruções remotamente para a VM instalar as ferramentas, autenticar no ACR, gerar a imagem Docker da API e enviá-la para a nuvem.

* **Inicie o processo de Build e Push remotamente:**

```bash
./03_5_build_e_push_vm.sh

```

*(Aguarde o processo finalizar. Pode levar alguns minutos até a confirmação de sucesso aparecer na tela do Cloud Shell).*

### 4. Deploy dos Containers (ACI)

Com a imagem pronta no ACR, suba os serviços finais que irão compor o sistema:

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



```

### Um detalhe importante antes de você rodar:
Dentro do próprio arquivo `03_5_build_e_push_vm.sh` que você tem, lembre-se de alterar a variável `REPO_URL` para o link real do seu repositório, caso contrário a automação vai tentar baixar de um repositório que não existe.

A variável dentro do script `.sh` deve ficar assim:
`REPO_URL="[https://github.com/Portifolio-Pamella/Challenge-Cloud-Devops.git](https://github.com/Portifolio-Pamella/Challenge-Cloud-Devops.git)"`

```