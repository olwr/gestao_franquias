using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestao_Franquias.Api.Migrations
{
    /// <inheritdoc />
    public partial class CorrigeDefaultAtivoFranqueadora : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Ativo",
                table: "Franqueadoras",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");
            
            // Corrige os registros que foram inseridos antes desta correção,
            // que herdaram o default incorreto (false) da migration anterior.
            migrationBuilder.Sql("UPDATE \"Franqueadoras\" SET \"Ativo\" = true WHERE \"Ativo\" = false;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Ativo",
                table: "Franqueadoras",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);
        }
    }
}
