-- Habilita extensão UUID (caso ainda não esteja)
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Tabela de departamentos
CREATE TABLE IF NOT EXISTS departamentos (
    codigo TEXT PRIMARY KEY,
    descricao TEXT NOT NULL
);

-- Inserir dados fixos de departamentos (idempotente)
INSERT INTO departamentos (codigo, descricao) VALUES
('010', 'BEBIDAS'),
('020', 'CONGELADOS'),
('030', 'LATICINIOS'),
('040', 'VEGETAIS')
ON CONFLICT (codigo) DO NOTHING;

-- Tabela de produtos
CREATE TABLE IF NOT EXISTS produtos (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    codigo TEXT NOT NULL UNIQUE,
    descricao TEXT NOT NULL,
    departamento_codigo TEXT NOT NULL REFERENCES departamentos(codigo),
    preco NUMERIC(12,2) NOT NULL CHECK (preco >= 0),
    status BOOLEAN NOT NULL DEFAULT TRUE,
    excluido BOOLEAN NOT NULL DEFAULT FALSE,
    criado_em TIMESTAMP WITH TIME ZONE DEFAULT now(),
    alterado_em TIMESTAMP WITH TIME ZONE
);
