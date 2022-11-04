using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroService.Migrations
{
    public partial class att : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Data_De_Criacao",
                table: "Leitores",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Leitores",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data_De_Criacao",
                table: "Leitores");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Leitores");
        }
    }
}
