using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestao_Franquias.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaAtivoFranqueadora : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Franqueadoras",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Franqueadoras_Cnpj",
                table: "Franqueadoras",
                column: "Cnpj",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Franqueadoras_Cnpj",
                table: "Franqueadoras");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Franqueadoras");
        }
    }
}
