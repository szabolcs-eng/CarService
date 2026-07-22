using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarServiceApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTechnicalInspectionExpiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TechnicalInspectionExpiry",
                table: "Vehicles",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TechnicalInspectionExpiry",
                table: "Vehicles");
        }
    }
}
