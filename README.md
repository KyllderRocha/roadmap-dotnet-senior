# Jornada .NET a Nível Sênior: Construindo um Task Manager

![.NET CI/CD](https://github.com/KyllderRocha/roadmap-dotnet-senior/actions/workflows/dotnet-ci.yml/badge.svg)

Este repositório documenta a jornada de aprendizagem e desenvolvimento de uma API RESTful completa em .NET, seguindo um roadmap evolutivo por fases. O objetivo é aplicar conceitos de arquitetura de software, boas práticas e ferramentas modernas para construir uma solução robusta, escalável e pronta para produção.

## 🚀 Roadmap do Projeto

Este projeto é dividido em fases, cada uma introduzindo novos conceitos e tecnologias, simulando o crescimento e a evolução de uma aplicação no mundo real.

---

### ✅ Fase 1: Fundamentos da API RESTful

**Objetivo:** Construir o MVP (Minimum Viable Product) de uma API, estabelecendo uma base sólida com as tecnologias essenciais do ecossistema .NET.

**Principais Tópicos e Tecnologias:**
- ASP.NET Core para a criação de APIs.
- Padrão Repository e Unit of Work para abstração do acesso a dados.
- Entity Framework Core como ORM.
- Banco de dados SQLite para simplicidade no desenvolvimento inicial.
- CRUD completo para Utilizadores e Tarefas.

---

### ✅ Fase 2: Arquitetura, Robustez e DevOps

**Objetivo:** Refatorar a aplicação para uma arquitetura limpa (Clean Architecture), introduzir robustez com validações e logs, e automatizar o processo de build e entrega.

**Principais Tópicos e Tecnologias:**
- **Clean Architecture:** Separação da solução em camadas (`Domain`, `Application`, `Infrastructure`, `Api`).
- **CQRS com MediatR:** Separação de responsabilidades de leitura (Queries) e escrita (Commands).
- **Docker & Docker Compose:** Containerização da aplicação e do banco de dados (PostgreSQL), garantindo um ambiente de desenvolvimento consistente.
- **Validação com FluentValidation:** Implementação de um pipeline de validação robusto e centralizado.
- **Tratamento de Exceções:** Criação de um middleware global para tratamento de exceções de negócio.
- **Logging Estruturado com Serilog:** Configuração de logs para a consola e ficheiros.
- **CI/CD Básico com GitHub Actions:** Automação do build e publicação da imagem Docker no GitHub Container Registry.

---

### Active **Fase 3: API Segura e Pronta para Produção (Fase Atual)**

**Objetivo:** Implementar um sistema completo de autenticação e autorização, transformando a API numa aplicação multi-utilizador segura.

**Principais Tópicos e Tecnologias:**
- **Autenticação vs. Autorização:** Aplicação prática dos conceitos.
- **JWT (JSON Web Tokens):** Geração e validação de tokens para autenticação *stateless*.
- **Hashing de Senhas com BCrypt:** Armazenamento seguro de credenciais de utilizador.
- **Autorização a Nível de Recurso:** Garantir que um utilizador só pode aceder e modificar os seus próprios dados.
- **Refatoração para `ICurrentUserService`:** Abstração para obter o utilizador autenticado de forma limpa e testável.
- **Configuração do Swagger para JWT:** Permitir o teste de endpoints protegidos.

---

### ➡️ **Fase 4: A Visão Sistémica com Microserviços (Próxima Fase)**

**Objetivo:** Quebrar a aplicação monolítica em partes menores, introduzir comunicação assíncrona e preparar para um ambiente de nuvem.

**Principais Tópicos e Tecnologias:**
- **Arquitetura de Microserviços:** Decomposição do sistema em serviços independentes (ex: `UserService`, `TaskService`).
- **Comunicação Assíncrona com RabbitMQ:** Uso de um *message broker* para comunicação fiável e desacoplada entre os serviços através de eventos.
- **Orquestração de Múltiplos Contentores:** Evolução do `docker-compose.yml` para gerir o novo ecossistema distribuído.
- **(Bônus) API Gateway:** Introdução de um ponto de entrada único para a API.
- **(Bônus) Infraestrutura como Código (IaC):** Primeiros passos com Terraform para provisionar recursos.

---

## 🛠️ Tecnologias Utilizadas

- **Framework:** .NET 8
- **Arquitetura:** Clean Architecture, CQRS, REST
- **Banco de Dados:** PostgreSQL
- **Containerização:** Docker, Docker Compose
- **Autenticação:** JWT (JSON Web Tokens)
- **Validação:** FluentValidation
- **Logging:** Serilog
- **CI/CD:** GitHub Actions

## ⚙️ Como Executar o Projeto (Fase 3)

1. **Clonar o repositório:**
   ```sh
   git clone [https://github.com/KyllderRocha/roadmap-dotnet-senior.git](https://github.com/KyllderRocha/roadmap-dotnet-senior.git)
   cd roadmap-dotnet-senior/fase-2-task-manager-clean 
   ```
   *(Nota: O projeto da Fase 3 é uma evolução do código da Fase 2)*

2. **Configurar a Connection String:**
   O ficheiro `docker-compose.yml` já está configurado para subir a API e o banco de dados PostgreSQL. A `ConnectionString` para o ambiente Docker é injetada via variáveis de ambiente.

3. **Subir os contentores:**
   ```sh
   docker-compose up --build
   ```

4. **Aplicar as Migrations:**
   Com os contentores a correr, abra um novo terminal e execute o comando para criar as tabelas na base de dados:
   ```sh
   dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/UserService.Api
   dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskService.Api
   ```

5. **Aceder à API:**
   A API estará disponível em `http://localhost:8080`. A documentação do Swagger (que permite testar todos os endpoints) pode ser acedida em `http://localhost:8080/swagger`.