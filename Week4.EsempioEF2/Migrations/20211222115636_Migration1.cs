using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Week4.EsempioEF2.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aziende",
                columns: table => new
                {
                    AziendaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnnoFondazione = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aziende", x => x.AziendaID);
                });

            migrationBuilder.CreateTable(
                name: "Impiegati",
                columns: table => new
                {
                    ImpiegatoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataNascita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AziendaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impiegati", x => x.ImpiegatoID);
                    table.ForeignKey(
                        name: "FK_Impiegati_Aziende_AziendaID",
                        column: x => x.AziendaID,
                        principalTable: "Aziende",
                        principalColumn: "AziendaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Impiegati_AziendaID",
                table: "Impiegati",
                column: "AziendaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Impiegati");

            migrationBuilder.DropTable(
                name: "Aziende");
        }
    }
}
