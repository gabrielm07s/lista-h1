#!/usr/bin/env bash
set -euo pipefail

base_url="http://127.0.0.1:5099"
response="$(mktemp)"
api_log="$(mktemp)"

dotnet run --project Uniaraxa.API --no-build --no-restore --urls "$base_url" >"$api_log" 2>&1 &
api_pid=$!
trap 'kill "$api_pid" 2>/dev/null || true; rm -f "$response" "$api_log"' EXIT

for attempt in {1..30}; do
  if curl -fsS "$base_url/api/teacher" >"$response"; then
    break
  fi
  sleep 1
done

if ! jq -e '. == []' "$response" >/dev/null; then
  cat "$api_log"
  exit 1
fi

check_status() {
  local expected="$1"
  shift
  local actual
  actual="$(curl -sS -o "$response" -w '%{http_code}' "$@")"
  if [[ "$actual" != "$expected" ]]; then
    printf 'Esperado HTTP %s, recebido %s\n' "$expected" "$actual" >&2
    cat "$response" >&2
    exit 1
  fi
}

check_status 201 -X POST "$base_url/api/teacher" -H 'Content-Type: application/json' -d '{"name":"Ana Silva","eMail":"ana@example.com","urlLattes":"https://lattes.cnpq.br/1234567890123456","dataAdmissao":"2026-02-01"}'
jq -e '.id == 1 and .eMail == "ana@example.com"' "$response" >/dev/null

check_status 400 -X POST "$base_url/api/teacher" -H 'Content-Type: application/json' -d '{"name":"Inválido","eMail":"sem-arroba","urlLattes":"https://lattes.cnpq.br/1234567890123456","dataAdmissao":"2026-02-01"}'

check_status 200 "$base_url/api/teacher/1"
jq -e '.name == "Ana Silva"' "$response" >/dev/null

check_status 200 "$base_url/api/teacher"
jq -e 'length == 1' "$response" >/dev/null

check_status 204 -X PUT "$base_url/api/teacher/1" -H 'Content-Type: application/json' -d '{"id":1,"name":"Ana Souza","eMail":"ana.souza@example.com","urlLattes":"https://lattes.cnpq.br/1234567890123456","dataAdmissao":"2026-02-01"}'
check_status 200 "$base_url/api/teacher/1"
jq -e '.name == "Ana Souza"' "$response" >/dev/null

check_status 400 -X PUT "$base_url/api/teacher/1" -H 'Content-Type: application/json' -d '{"id":2,"name":"Ana Souza","eMail":"ana.souza@example.com","urlLattes":"https://lattes.cnpq.br/1234567890123456","dataAdmissao":"2026-02-01"}'

check_status 204 -X DELETE "$base_url/api/teacher/1"
check_status 404 "$base_url/api/teacher/1"
check_status 404 -X DELETE "$base_url/api/teacher/1"
