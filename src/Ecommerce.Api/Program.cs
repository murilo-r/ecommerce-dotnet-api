using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Ecommerce.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ecommerce API", Version = "v1" });
            });

            builder.Services.AddScoped<IDbConnection>(sp =>
                new NpgsqlConnection(builder.Configuration.GetConnectionString("Default")));
            builder.Services.AddCors(p => p.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors();

            app.MapGet("/api/departamentos", async (IDbConnection db) =>
            {
                const string sql = "SELECT codigo, descricao FROM departamentos ORDER BY descricao";
                var list = await db.QueryAsync<DepartamentoDto>(sql);
                return Results.Ok(list);
            });

            app.MapGet("/api/produtos", async (IDbConnection db) =>
            {
                const string sql = @"
                    SELECT id, codigo, descricao, departamento_codigo AS DepartamentoCodigo,
                           preco, status
                    FROM produtos
                    WHERE excluido = FALSE
                    ORDER BY descricao";
                var list = await db.QueryAsync<ProdutoDto>(sql);
                return Results.Ok(list);
            });

            app.MapPost("/api/produtos", async (IDbConnection db, ProdutoCreateUpdateDto dto) =>
            {
                if (string.IsNullOrWhiteSpace(dto.Codigo) || string.IsNullOrWhiteSpace(dto.Descricao))
                    return Results.BadRequest("Código e descrição são obrigatórios.");

                const string insertSql = @"
                    INSERT INTO produtos (id, codigo, descricao, departamento_codigo, preco, status, excluido)
                    VALUES (@Id, @Codigo, @Descricao, @DepartamentoCodigo, @Preco, @Status, FALSE)
                    ON CONFLICT (codigo) DO UPDATE SET
                        descricao = EXCLUDED.descricao,
                        departamento_codigo = EXCLUDED.departamento_codigo,
                        preco = EXCLUDED.preco,
                        status = EXCLUDED.status,
                        alterado_em = now();
                ";

                var produtoId = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id;

                await db.ExecuteAsync(insertSql, new
                {
                    Id = produtoId,
                    Codigo = dto.Codigo,
                    Descricao = dto.Descricao,
                    DepartamentoCodigo = dto.DepartamentoCodigo,
                    Preco = dto.Preco,
                    Status = dto.Status
                });

                return Results.Ok(new { Id = produtoId });
            });

            app.MapDelete("/api/produtos/{id:guid}", async (IDbConnection db, Guid id) =>
            {
                const string deleteSql = @"
                    UPDATE produtos
                    SET excluido = TRUE, alterado_em = now()
                    WHERE id = @Id;
                ";
                var affected = await db.ExecuteAsync(deleteSql, new { Id = id });
                if (affected == 0)
                    return Results.NotFound();
                return Results.NoContent();
            });

            await app.RunAsync();
        }
    }

    public record DepartamentoDto(string Codigo, string Descricao);
    public record ProdutoDto(Guid Id, string Codigo, string Descricao, string DepartamentoCodigo, decimal Preco, bool Status);
    public record ProdutoCreateUpdateDto(Guid Id, string Codigo, string Descricao, string DepartamentoCodigo, decimal Preco, bool Status);
}
