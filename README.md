# Cadastro de professores — Uniaraxá

Implementação em C# baseada no [projeto da aula](https://github.com/1bertomelo/2026_2_Uniaraxa), com a entidade `Teacher` e cinco operações completas nas camadas Domain, Infra, Service e API.

## O que foi feito

1. A estrutura inicial da aula foi importada para este repositório pessoal.
2. Uma chave excedente em `StudentService.cs` foi removida para corrigir a sintaxe do projeto original.
3. `Teacher` foi criado com `Id`, `Name`, `Email` (JSON: `eMail`), `UrlLattes` (JSON: `urlLattes`) e `DataAdmissao` (JSON: `dataAdmissao`).
4. `ITeacherRepository` e `TeacherRepository` implementam inserção, atualização, listagem, busca por ID e remoção. O repositório usa um dicionário em memória e sincroniza o acesso entre requisições.
5. `ITeacherService` e `TeacherService` fazem a ligação entre a API e o repositório.
6. `TeacherController` expõe os cinco endpoints HTTP. O `Program.cs` registra as dependências e a API referencia o projeto Infra.

## Requisitos e execução

Instale o **SDK do .NET 10**. O runtime sozinho não inclui os comandos necessários para compilar. Na raiz do repositório:

```powershell
dotnet restore Uniaraxa.slnx
dotnet build Uniaraxa.slnx
dotnet run --project Uniaraxa.API --urls http://localhost:5000
```

O workflow `.github/workflows/build.yml` executa restore e build com .NET 10 a cada push em `main` e em pull requests.

Na configuração de desenvolvimento, o documento OpenAPI fica em `/openapi/v1.json`.

## API de Teacher

| Operação | Método e rota | Resultado |
| --- | --- | --- |
| Inserir | `POST /api/teacher` | `201 Created`, com `Location` e o professor criado |
| Listar todos | `GET /api/teacher` | `200 OK`, inclusive lista vazia |
| Buscar por ID | `GET /api/teacher/{id}` | `200 OK` ou `404 Not Found` |
| Atualizar | `PUT /api/teacher/{id}` | `204 No Content`, `400 Bad Request` ou `404 Not Found` |
| Remover | `DELETE /api/teacher/{id}` | `204 No Content` ou `404 Not Found` |

Exemplo de corpo para `POST` (o servidor atribui o `id`):

```json
{
  "name": "Ana Silva",
  "eMail": "ana.silva@example.com",
  "urlLattes": "https://lattes.cnpq.br/1234567890123456",
  "dataAdmissao": "2026-02-01"
}
```

Exemplo de uso:

```powershell
curl.exe -i -X POST http://localhost:5000/api/teacher -H "Content-Type: application/json" -d '{"name":"Ana Silva","eMail":"ana.silva@example.com","urlLattes":"https://lattes.cnpq.br/1234567890123456","dataAdmissao":"2026-02-01"}'
curl.exe -i http://localhost:5000/api/teacher
curl.exe -i http://localhost:5000/api/teacher/1
curl.exe -i -X PUT http://localhost:5000/api/teacher/1 -H "Content-Type: application/json" -d '{"id":1,"name":"Ana Souza","eMail":"ana.souza@example.com","urlLattes":"https://lattes.cnpq.br/1234567890123456","dataAdmissao":"2026-02-01"}'
curl.exe -i -X DELETE http://localhost:5000/api/teacher/1
```

`name`, `eMail` e `urlLattes` são obrigatórios; `eMail` precisa ter formato de e-mail e `urlLattes` precisa ser uma URL válida. `dataAdmissao` precisa ser uma data ISO (`AAAA-MM-DD`). Na atualização, se o corpo trouxer `id`, ele deve corresponder ao ID da rota.

## Limitação do exemplo

O repositório de Teacher guarda os dados somente na memória do processo. Os registros desaparecem quando a aplicação reinicia. O projeto de Student foi preservado como referência da aula; seu repositório original ainda contém métodos não implementados.

## Histórico de commits

Cada etapa foi registrada em um commit próprio: importação do projeto da aula, correção da sintaxe existente, implementação das camadas de Teacher, exposição dos endpoints, documentação e verificação de build.

## Referências

- [Projeto da aula](https://github.com/1bertomelo/2026_2_Uniaraxa)
- [APIs com controllers no ASP.NET Core](https://learn.microsoft.com/aspnet/core/web-api/?view=aspnetcore-10.0)
- [Injeção de dependências no ASP.NET Core](https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-10.0)
