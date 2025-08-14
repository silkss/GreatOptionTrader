using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreatOptionTrader.Migrations
{
    /// <inheritdoc />
    public partial class newtypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Instruments_InstrumentId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Instruments");

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InstrumentGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    Exchange = table.Column<string>(type: "TEXT", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Multiplier = table.Column<int>(type: "INTEGER", nullable: false),
                    PriceMagnifier = table.Column<int>(type: "INTEGER", nullable: false),
                    Strike = table.Column<decimal>(type: "TEXT", nullable: false),
                    Right = table.Column<int>(type: "INTEGER", nullable: false),
                    TradingClass = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Options_InstrumentGroups_InstrumentGroupId",
                        column: x => x.InstrumentGroupId,
                        principalTable: "InstrumentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Options_InstrumentGroupId",
                table: "Options",
                column: "InstrumentGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Options_InstrumentId",
                table: "Orders",
                column: "InstrumentId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Options_InstrumentId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.CreateTable(
                name: "Instruments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Exchange = table.Column<string>(type: "TEXT", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InstrumentGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    Multiplier = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Strike = table.Column<double>(type: "REAL", nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instruments_InstrumentGroups_InstrumentGroupId",
                        column: x => x.InstrumentGroupId,
                        principalTable: "InstrumentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_InstrumentGroupId",
                table: "Instruments",
                column: "InstrumentGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Instruments_InstrumentId",
                table: "Orders",
                column: "InstrumentId",
                principalTable: "Instruments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
