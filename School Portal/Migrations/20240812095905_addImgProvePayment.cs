using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Portal.Migrations
{
    public partial class addImgProvePayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "PaymentModels",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "PaymentModels");
        }
    }
}
