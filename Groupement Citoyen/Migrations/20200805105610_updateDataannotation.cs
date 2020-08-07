using Microsoft.EntityFrameworkCore.Migrations;

namespace Groupement_Citoyen.Migrations
{
    public partial class updateDataannotation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Poids",
                table: "Produits");

            migrationBuilder.DropColumn(
                name: "MotDePasse",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<decimal>(
                name: "Prix",
                table: "Produits",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrixUnitaire",
                table: "DetailsCommandes",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Commandes",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<decimal>(
                name: "MontantPortefeuille",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Prix",
                table: "Produits",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<float>(
                name: "Poids",
                table: "Produits",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<float>(
                name: "PrixUnitaire",
                table: "DetailsCommandes",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<float>(
                name: "Total",
                table: "Commandes",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<float>(
                name: "MontantPortefeuille",
                table: "AspNetUsers",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<string>(
                name: "MotDePasse",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
