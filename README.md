# Task Manager 

Sistema full stack de gerenciamento pessoal de tarefas desenvolvido como teste técnico para vaga de Desenvolvedor Júnior.

---

## Tecnologias Utilizadas

| Camada | Tecnologia |
|---|---|
| Frontend | Angular 15+ |
| Backend | ASP.NET Core 8 + Entity Framework Core |
| Banco de Dados | PostgreSQL 15 |
| Integração | Node-RED |

---

## Estrutura do Repositório

```
/
├── frontend/          # Aplicação Angular
├── backend/           # API ASP.NET Core
├── database/          # Script SQL de inicialização
├── nodered/           # Fluxos Node-RED exportados (flows.json)
└── README.md
```

---

## Pré-requisitos

Certifique-se de ter instalado:

- [Node.js](https://nodejs.org/) (v18+)
- [Angular CLI](https://angular.io/cli): `npm install -g @angular/cli`
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- [Node-RED](https://nodered.org/): `npm install -g node-red`

---

## Banco de Dados

### 1. Criar o banco no pgAdmin

1. Abra o **pgAdmin** e conecte ao servidor
2. Botão direito em **Databases → Create -> Database**
3. Nome: `taskmanager` -> **Save**

### 2. Executar o script SQL

No pgAdmin, selecione o banco `taskmanager`:

```
Tools → Query Tool → abra o arquivo database/taskmanager.sql -> Execute
```

Ou pelo terminal:

```bash
psql -U postgres -d taskmanager -f database/taskmanager.sql
```

### Modelo de Dados

```sql
tasks (
  id          SERIAL PRIMARY KEY,
  title       VARCHAR(255) NOT NULL,
  description TEXT,
  priority    VARCHAR(20)  NOT NULL DEFAULT 'normal',  -- baixa | normal | alta
  status      VARCHAR(30)  NOT NULL DEFAULT 'nao_iniciado', -- nao_iniciado | em_progresso | concluido
  created_at  TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
  updated_at  TIMESTAMPTZ  NOT NULL DEFAULT NOW()
)
```

---

## Executando o Backend

### 1. Configurar a connection string

Abra `backend/appsettings.json` e ajuste com seus dados do PostgreSQL:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=taskmanager;Username=postgres;Password=SUA_SENHA"
  }
}
```

### 2. Restaurar dependências e rodar

```bash
cd backend
dotnet restore
dotnet run
```

A API estará disponível em: **http://localhost:5000**

---

##  Endpoints da API

Base URL: `http://localhost:5000`

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/task` | Listar todas as tarefas |
| GET | `/api/task/{id}` | Buscar tarefa por ID |
| POST | `/api/task` | Criar nova tarefa |
| PUT | `/api/task/{id}` | Atualizar tarefa existente |
| DELETE | `/api/task/{id}` | Remover tarefa |

### Swagger UI

Documentação interativa disponível em:

```
http://localhost:5000/swagger
```

### Exemplo de payload – POST /api/task

```json
{
  "title": "Implementar testes unitários",
  "description": "Cobrir controllers e serviços com xUnit",
  "priority": "alta",
  "status": "nao_iniciado"
}
```

### Valores aceitos

| Campo | Valores |
|-------|---------|
| `priority` | `baixa` \| `normal` \| `alta` |
| `status` | `nao_iniciado` \| `em_progresso` \| `concluido` |

### Exemplo de resposta – GET /api/task

```json
[
  {
    "id": 1,
    "title": "Implementar testes unitários",
    "description": "Cobrir controllers e serviços com xUnit",
    "priority": "alta",
    "status": "nao_iniciado",
    "createdAt": "2024-03-19T10:00:00Z",
    "updatedAt": "2024-03-19T10:00:00Z"
  }
]
```

---

## Executando o Frontend

```bash
cd frontend
npm install
ng serve
```

A aplicação estará disponível em: **http://localhost:4200**

### Funcionalidades

- Listagem de tarefas em cards
- Filtro por status e prioridade
- Criar tarefa via modal
- Editar tarefa via modal
- Excluir tarefa com confirmação
- Badges coloridos por prioridade e status

---

## Executando o Node-RED

### 1. Iniciar o Node-RED

```bash
node-red
```

Acesse a interface em: **http://localhost:1880**

### 2. Importar os fluxos

1. No Node-RED, clique no menu **≡ (canto superior direito)**
2. Clique em **Import**
3. Selecione o arquivo `nodered/flows.json`
4. Clique em **Import**
5. Clique em **Implementar** (botão vermelho)

### Rotas disponíveis

| Rota | Descrição |
|------|-----------|
| `http://localhost:1880/corretora` | Catálogo de corretoras da BrasilAPI com busca |
| `http://localhost:1880/cep/:cep` | Busca de CEP com exibição de endereço |

### Exemplos de uso

```
# Listar todas as corretoras
http://localhost:1880/corretora

# Buscar um CEP específico
http://localhost:1880/cep/30140071
```

### APIs externas consumidas

| Fluxo | API |
|-------|-----|
| Corretoras | `https://brasilapi.com.br/api/cvm/corretoras/v1` |
| CEP | `https://brasilapi.com.br/api/cep/v2/{cep}` |

---

## Resumo de Portas

| Serviço | URL |
|---------|-----|
| Frontend Angular | http://localhost:4200 |
| Backend API | http://localhost:5000 |
| Swagger UI | http://localhost:5000/swagger |
| Node-RED | http://localhost:1880 |
| Corretoras | http://localhost:1880/corretora |
| CEP | http://localhost:1880/cep/30140071 |

---

## Observações

- O backend cria as tabelas automaticamente ao iniciar (`EnsureCreated`)
- O frontend se comunica com o backend via `http://localhost:5000`
- Os fluxos Node-RED são independentes do sistema de tarefas
- Certifique-se de que o PostgreSQL está rodando antes de iniciar o backend
