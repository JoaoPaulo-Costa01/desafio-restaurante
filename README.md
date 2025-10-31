# Dashboard de Análise de Vendas (Desafio Nola)

Este projeto é uma API RESTful e um Dashboard de Front-end para um sistema de análise de dados de restaurantes, desenvolvido para o "God Level Coder Challenge".

A aplicação permite que um usuário explore dados de vendas de forma interativa, filtrando e visualizando métricas de performance em tempo real. O sistema foi desenvolvido para rodar localmente contra um banco de dados PostgreSQL populado com mais de 500.000 registros de vendas.

## 🚀 Tecnologias Utilizadas

Este projeto foi construído com as seguintes tecnologias:

* **C# / .NET 8:** Framework para a construção da Web API (Back-end).
* **Entity Framework (EF) Core 8:** ORM (Object-Relational Mapper) para a comunicação com o banco de dados.
* **PostgreSQL 16:** SGBD (Sistema Gerenciador de Banco de Dados) relacional.
* **React 18 (com Vite):** Biblioteca para a construção da interface de usuário (Front-end).
* **Chart.js:** Biblioteca para a visualização de dados e criação dos gráficos.
* **Axios:** Biblioteca para realizar as chamadas HTTP (requests) do Front-end para o Back-end.
* **Swagger (OpenAPI):** Ferramenta utilizada para documentar e testar os endpoints da API.

## ✨ Funcionalidades (Endpoints da API)

A API (`/api/analytics`) expõe os seguintes endpoints otimizados:

* **[GET] `/api/analytics/top-products`**
    * **Descrição:** Retorna o Top 10 produtos mais vendidos.
    * **Parâmetros:** `channelId` (int), `dayOfWeek` (DayOfWeek).

* **[GET] `/api/analytics/ticket-medio-por-canal`**
    * **Descrição:** Retorna o valor médio do ticket (R$) por canal, agrupado por dia.
    * **Parâmetros:** `startDate` (DateTime), `endDate` (DateTime).

* **[GET] `/api/analytics/tempo-medio-entrega`**
    * **Descrição:** Retorna o tempo médio de entrega (em minutos) agrupado por dia.
    * **Parâmetros:** `startDate` (DateTime), `endDate` (DateTime).

* **[GET] `/api/analytics/clientes-inativos`**
    * **Descrição:** Retorna uma lista de clientes fiéis que estão inativos.
    * **Parâmetros:** `minCompras` (int), `diasInativo` (int).

## ⚙️ Como Executar o Projeto (Setup Local)

### Pré-requisitos:

* .NET 8 SDK
* Visual Studio 2022 (com workload de ASP.NET)
* PostgreSQL 16 (instalado localmente, com pgAdmin)
* Node.js (v18+)

### 1. Preparar o Banco de Dados (PostgreSQL)

**Crie o Banco:**

* Abra o pgAdmin, conecte-se ao seu servidor local.
* Clique com o botão direito em "Databases" -> "Create" -> "Database...".
* Nomeie o banco: `challenge_db`.

**Crie as Tabelas:**

* Abra a "Query Tool" para o `challenge_db`.
* Execute o script SQL (fornecido no desafio, começando com `CREATE TABLE brands...`) para criar todas as 16 tabelas.

**Popule o Banco:**

* Abra o script `generate_data.py` (do repositório original do desafio).
* Edite a função `get_db_connection` para apontar para o seu banco local:
    ```python
    def get_db_connection(db_url):
        return psycopg2.connect(
            host="localhost",
            database="challenge_db",
            user="postgres",
            password="SUA_SENHA" 
        )
    ```
* Abra um terminal na pasta do script `generate_data.py` e rode:
    ```bash
    python generate_data.py --months 6 --stores 50 --products 500 --items 200 --customers 10000
    ```
* Aguarde 10-15 minutos até os 500k+ de vendas serem gerados.

### 2. Rodar o Back-end (API C#)

**Abra o Projeto:**

* Abra o arquivo `.sln` do projeto de Back-end (ex: `NolaProject.sln`) com o Visual Studio 2022.

**Configure a Conexão:**

* Abra o arquivo `appsettings.json`.
* Verifique se a `ConnectionStrings` ("DefaultConnection") está correta para o seu banco `challenge_db` local (com sua senha).

**Execute a Aplicação:**

* Aperte F5 (ou o botão "Play") para iniciar a API.
* Uma janela do Swagger deve abrir (ex: `https://localhost:7180/`). Anote esta porta.

### 3. Rodar o Front-end (React)

**Abra o Projeto:**

* Abra a pasta do projeto de Front-end (ex: `nola-dashboard`) no VS Code.

**Verifique a API URL:**

* Abra o arquivo `src/App.jsx`.
* Verifique se a `apiUrl` (ex: `https://localhost:7180`) está apontando para a porta correta da sua API.

**Instale as dependências (se for a primeira vez):**

```bash
npm install
```
**Execute a Aplicação:**

* Abra um terminal no VS Code e rode:

```bash
npm run dev
```
* Teste no Navegador:

Abra o endereço que o terminal indicar (ex: http://localhost:5173) no seu navegador.
