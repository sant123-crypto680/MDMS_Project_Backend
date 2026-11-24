using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MDMS_PRoject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddConsumptionMetrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageConsumption",
                table: "ConsumptionSeries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxUsage",
                table: "ConsumptionSeries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinUsage",
                table: "ConsumptionSeries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalConsumption",
                table: "ConsumptionSeries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageConsumption",
                table: "ConsumptionSeries");

            migrationBuilder.DropColumn(
                name: "MaxUsage",
                table: "ConsumptionSeries");

            migrationBuilder.DropColumn(
                name: "MinUsage",
                table: "ConsumptionSeries");

            migrationBuilder.DropColumn(
                name: "TotalConsumption",
                table: "ConsumptionSeries");
        }
    }
}
