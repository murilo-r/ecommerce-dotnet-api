# Quickstart - Backend (API .NET + PostgreSQL)

## Pré-requisitos

- .NET 7 SDK
- PostgreSQL rodando
- Terminal (ex: PowerShell no Windows)

## 1. Criar o banco de dados

```powershell
# Exemplo usando usuário padrão postgres (ajuste usuário/senha se necessário)
createdb -U postgres ecommerce_basic
```

## 2. Aplicar schema e dados iniciais

```powershell
psql -U postgres -d ecommerce_basic -f schema.sql
```
