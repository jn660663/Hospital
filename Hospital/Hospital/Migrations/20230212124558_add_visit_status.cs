using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Migrations
{
    public partial class add_visit_status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Examinations");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Visits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Visits");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Examinations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
