using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroService.Migrations
{
    public partial class AtivacaoDeConta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Leitores",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Codigo_De_Ativacao",
                table: "Leitores",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Leitores");

            migrationBuilder.DropColumn(
                name: "Codigo_De_Ativacao",
                table: "Leitores");
        }
    }
}
