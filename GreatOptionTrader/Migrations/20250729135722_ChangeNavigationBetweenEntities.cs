using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreatOptionTrader.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNavigationBetweenEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instruments_InstrumentGroups_InstrumentGroupId",
                table: "Instruments");

            migrationBuilder.AddForeignKey(
                name: "FK_Instruments_InstrumentGroups_InstrumentGroupId",
                table: "Instruments",
                column: "InstrumentGroupId",
                principalTable: "InstrumentGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instruments_InstrumentGroups_InstrumentGroupId",
                table: "Instruments");

            migrationBuilder.AddForeignKey(
                name: "FK_Instruments_InstrumentGroups_InstrumentGroupId",
                table: "Instruments",
                column: "InstrumentGroupId",
                principalTable: "InstrumentGroups",
                principalColumn: "Id");
        }
    }
}
