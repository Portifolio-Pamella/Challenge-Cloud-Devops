```markdown
# Sistema Veterinário API

Este projeto é uma API RESTful desenvolvida em **ASP.NET Core** para o gerenciamento de uma clínica veterinária. O sistema permite o controle de **Tutores**, seus **Pets** e os **Veterinários** responsáveis, garantindo a integridade dos dados através de uma arquitetura em camadas e integração com banco de dados **Oracle**. 

A aplicação conta também com recursos avançados de observabilidade corporativa, incluindo **Health Checks**, **Logging Estruturado com Serilog** e **Tracing/Métricas com OpenTelemetry**, além de uma suíte completa de **testes unitários e de integração**.

## Integrantes
* **Felipe Ribeiro Salles de Camargo | RM565224
* **Lucas Matsubara Reis** | RM565020
* **João Pedro Camilo** | RM562005
* **Pamella Christiny** | RM565206

## Tecnologias Utilizadas
* **ASP.NET Core 10.0**
* **Entity Framework Core** (ORM)
* **Oracle Database**
* **Swagger (OpenAPI)** para documentação
* **OpenTelemetry** para rastreamento distribuído e métricas de desempenho
* **Serilog** para logging estruturado
* **xUnit & Moq** para testes automatizados

---

## Pré-requisitos
Para executar o projeto, certifique-se de ter instalado:
* [.NET SDK 10.0](https://dotnet.microsoft.com/download/dotnet/10.0)
* [Oracle Database](https://www.oracle.com/br/database/technologies/oracle-database-software-downloads.html) (Xe, 19c ou superior)
* [SQL Developer](https://www.oracle.com/br/database/sqldeveloper/technologies/download.html)

## Como Executar
1. **Clone o repositório:**
   ```bash
   git clone git@github.com:Portifolio-Pamella/sistema-veterinario-dotnet.git

```

2. **Configuração do Banco:**
Crie um usuário no seu Oracle. No arquivo `appsettings.json`, atualize a `OracleConnection` com suas credenciais:
```json
"ConnectionStrings": {
  "OracleConnection": "User Id=USUARIO;Password=SENHA;Data Source=localhost:1521/xe;"
}

```


3. **Aplicar Migrations:**
No terminal, na pasta do projeto, execute:
```bash
dotnet ef database update

```


Isso criará as tabelas e executará as Triggers e Sequences necessárias.
4. **Executar:**
```bash
dotnet run --project .\SistemaVeterinario.API\SistemaVeterinario.API.csproj

```


Acesse: `http://localhost:5262/swagger`

## Como Executar os Testes

O projeto possui uma suíte organizada de testes unitários (com xUnit e Moq) e de integração (utilizando `WebApplicationFactory`). Para rodar todos os testes da solução, execute o comando na raiz do repositório:

```bash
dotnet test

```

## Monitoramento e Health Checks

A API dispõe de endpoints dedicados para verificação de saúde e disponibilidade da aplicação e de sua conectividade com o banco de dados:

* **Endpoints de Health Check**: Utilize os caminhos configurados na API (`/health/live` e `/health/ready`) para checar se o serviço e o Oracle estão operacionais.
* **OpenTelemetry & Logging**: O sistema coleta métricas de desempenho (tempo de resposta, taxa de erros, rastreamento de requisições entre camadas) e logs estruturados que são exibidos diretamente no console ou integrados a coletores compatíveis.

---

## Arquitetura do Projeto

O sistema foi construído seguindo uma Arquitetura em Camadas (Data, Models, Repositories, Services e Controllers):

* **Persistência**: Utiliza EF Core com mapeamento fluido. As chaves primárias são geridas pelo banco de dados através de Triggers e Sequences, garantindo autonomia do SGBD.
* **API**: Desenvolvida com o padrão REST, com tratamento global de erros (try-catch) para assegurar retornos HTTP semânticos (200, 201, 204, 400, 404).

## Documentação das Rotas

### Veterinário (`/api/Veterinario`)

* **GET**: Lista todos os veterinários.
* **POST**: Cadastra um novo veterinário.
* **GET `/{id}**`: Busca um veterinário específico.
* **PUT `/{id}**`: Atualiza um registro.
* **DELETE `/{id}**`: Remove um registro.

### Tutor (`/api/Tutor`)

* **GET**: Lista todos os tutores.
* **POST**: Cadastra um novo tutor.
* **GET `/{id}**`: Busca um tutor por ID.
* **PUT `/{id}**`: Atualiza um registro de tutor.
* **DELETE `/{id}**`: Remove um registro de tutor.

### Pet (`/api/Pet`)

* **GET**: Lista todos os pets.
* **POST**: Cadastra um novo pet vinculado a um tutor.
* **GET `/{id}**`: Busca um pet específico por ID.
* **PUT `/{id}**`: Atualiza os dados do pet.
* **DELETE `/{id}**`: Remove um registro de pet.

---

## Exemplos de JSON para Teste

### Cadastro de Veterinário

```json
{
  "nomeVeterinario": "Dra. Maria Teste",
  "crmVeterinario": "CRM-12345",
  "especialidadeVeterinario": "Felinos",
  "telefoneVeterinario": "11999999999",
  "emailVeterinario": "teste@email.com",
  "statusVeterinario": "Ativo",
  "dataCadastroVeterinario": "2026-05-23T21:00:00"
}

```

### Cadastro de Tutor

```json
{
  "nomeTutor": "João Silva",
  "cpfTutor": "123.456.789-00",
  "telefoneTutor": "(11) 99999-8888",
  "emailTutor": "joao.silva@email.com",
  "cepTutor": "01001-000",
  "ruaTutor": "Avenida Paulista",
  "numeroTutor": "1000",
  "complementoTutor": "Apto 52",
  "bairroTutor": "Bela Vista",
  "cidadeTutor": "São Paulo",
  "estadoTutor": "SP",
  "dataCadastroTutor": "2026-05-24T01:03:42.967Z"
}

```

### Cadastro de Pet

*(Certifique-se de usar um `idTutor` existente)*

```json
{
  "idTutor": 1,
  "nomePet": "Rex",
  "especiePet": "Cachorro",
  "racaPet": "Golden Retriever",
  "sexoPet": "Macho",
  "dataNascimentoPet": "2024-01-15T00:00:00Z",
  "pesoPet": 25.5,
  "corPet": "Dourado",
  "dataCadastroPet": "2026-05-24T01:03:42.967Z"
}

```

```

```