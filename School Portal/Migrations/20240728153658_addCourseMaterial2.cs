using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Portal.Migrations
{
    public partial class addCourseMaterial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseMaterials",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseMaterials",
                table: "Courses");
        }
    }
}
