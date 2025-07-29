using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreatOptionTrader.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ask",
                table: "Instruments");

            migrationBuilder.DropColumn(
                name: "Bid",
                table: "Instruments");

            migrationBuilder.AlterColumn<int>(
                name: "InstrumentGroupId",
                table: "Instruments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "InstrumentGroupId",
                table: "Instruments",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<double>(
                name: "Ask",
                table: "Instruments",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Bid",
                table: "Instruments",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
