# Ecommerce Admin API (.NET 7 + PostgreSQL)

## Visão geral

Este é o backend do módulo de administração de produtos de um e-commerce.  
Ele expõe uma API REST documentada com **Swagger**, persiste dados em **PostgreSQL** sem usar Entity Framework (usa Dapper com SQL explícito), e implementa:

- CRUD de produtos (com exclusão lógica)
- Endpoint para listar departamentos fixos
- Upsert de produto e separação lógica para soft delete

O nome do banco utilizado neste projeto é: **`ecommerce_basic`**.

---

## Recursos principais

- **Produtos**: ID (UUID), código, descrição, departamento, preço, status (ativo/inativo), exclusão lógica.
- **Departamentos**: Lista inicial consumível via API com dados no PostgreSQL (`010` Bebidas, `020` Congelados, `030` Laticínios, `040` Vegetais).
- **Swagger**: Interface interativa em tempo de execução.
- **Dapper**: Acesso a dados com queries explícitas.
- **Soft delete**: Produtos não são removidos fisicamente; ficam marcados.
- **Upsert**: POST em produtos cria ou atualiza pelo código.

---

## Pré-requisitos

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [PostgreSQL 13+](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/)
- (Opcional) Docker — para rodar PostgreSQL sem instalar localmente

---

## Configuração local

### 1. Clonar

```bash
git clone https://github.com/seu-usuario/ecommerce-dotnet-api.git
cd ecommerce-dotnet-api/src/Ecommerce.Api
```
