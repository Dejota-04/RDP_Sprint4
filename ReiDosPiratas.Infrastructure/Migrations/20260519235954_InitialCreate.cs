using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReiDosPiratas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PRODUTOS",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ENDERECO_IMAGEM = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PRECO = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PRECO_ORIGINAL = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PESO = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    ESTOQUE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CONDICAO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ALTURA = table.Column<float>(type: "BINARY_FLOAT", nullable: false),
                    LARGURA = table.Column<float>(type: "BINARY_FLOAT", nullable: false),
                    PROFUNDIDADE = table.Column<float>(type: "BINARY_FLOAT", nullable: false),
                    FUNCIONARIO_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    AUTOR = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CATEGORIA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTOS", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PRODUTOS");
        }
    }
}
