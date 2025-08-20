using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreatOptionTrader.Migrations
{
    /// <inheritdoc />
    public partial class addedmarketrules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MarketRulesId",
                table: "Options",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarketRulesId",
                table: "Options");
        }
    }
}
