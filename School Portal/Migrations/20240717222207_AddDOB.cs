using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Portal.Migrations
{
    public partial class AddDOB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StateOfOrigin",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateOfOrigin",
                table: "AspNetUsers");
        }
    }
}
