# Sistema Veterinário API

Este projeto é uma API RESTful desenvolvida em **ASP.NET Core** para o gerenciamento de uma clínica veterinária. O sistema permite o controle de **Tutores**, seus **Pets** e os **Veterinários** responsáveis, garantindo a integridade dos dados através de uma arquitetura em camadas e integração com banco de dados **Oracle**.

## Integrantes
* **Lucas Matsubara Reis** | RM565020
* **João Pedro Camilo** | RM562005
* **Pamella Christiny** | RM565206

## Tecnologias Utilizadas
* **ASP.NET Core 10.0**
* **Entity Framework Core** (ORM)
* **Oracle Database**
* **Swagger (OpenAPI)** para documentação

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
Configuração do Banco:

Crie um usuário no seu Oracle.

No arquivo appsettings.json, atualize a OracleConnection com suas credenciais:

JSON
"ConnectionStrings": {
  "OracleConnection": "User Id=USUARIO;Password=SENHA;Data Source=localhost:1521/xe;"
}

Aplicar Migrations:
No terminal, na pasta do projeto, execute:

Bash
dotnet ef database update
Isso criará as tabelas e executará as Triggers e Sequences necessárias.

Executar:

Bash
dotnet run
Acesse: http://localhost:5262/swagger

Arquitetura do Projeto
O sistema foi construído seguindo uma Arquitetura em Camadas (Data, Models, Repositories, Services e Controllers):

Persistência: Utiliza EF Core com mapeamento fluido. As chaves primárias são gerenciadas pelo banco de dados através de Triggers e Sequences, garantindo autonomia do SGBD.

API: Desenvolvida com o padrão REST, com tratamento global de erros (try-catch) para assegurar retornos HTTP semânticos (200, 201, 204, 400, 404).

Documentação das Rotas
Veterinário (/api/Veterinario)
GET: Lista todos os veterinários.

GET /{id}: Busca um veterinário específico.

GET /especialidade/{especialidade}: Filtra veterinários por especialidade.

POST: Cadastra um novo veterinário.

PUT /{id}: Atualiza um registro.

DELETE /{id}: Remove um registro.

Tutor (/api/Tutor)
GET: Lista todos os tutores.

GET /{id}: Busca um tutor por ID.

GET /cidade/{cidade}: Filtra tutores por cidade.

POST: Cadastra um novo tutor.

Pet (/api/Pet)
GET: Lista todos os pets.

GET /especie/{especie}: Filtra pets por espécie.

POST: Cadastra um pet vinculado a um idTutor.

PUT /{id}: Atualiza os dados do pet.

Exemplos de JSON para Teste
Cadastro de Veterinário
JSON
{
  "nomeVeterinario": "Dra. Maria Teste",
  "crmVeterinario": "CRM-12345",
  "especialidadeVeterinario": "Felinos",
  "telefoneVeterinario": "11999999999",
  "emailVeterinario": "teste@email.com",
  "statusVeterinario": "Ativo",
  "dataCadastroVeterinario": "2026-05-23T21:00:00"
}
Cadastro de Tutor
JSON
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
Cadastro de Pet
Certifique-se de usar um idTutor existente.

JSON
{
  "idTutor": 0,
  "nomePet": "Rex",
  "especiePet": "Cachorro",
  "racaPet": "Golden Retriever",
  "sexoPet": "Macho",
  "dataNascimentoPet": "2024-01-15T00:00:00Z",s
  "pesoPet": 25.5,
  "corPet": "Dourado",
  "dataCadastroPet": "2026-05-24T01:03:42.967Z"
}