using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Portal.Migrations
{
    public partial class addPaymentForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankPaidFrom",
                table: "PaymentModels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "PaymentModels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangeDate",
                table: "PaymentModels",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankPaidFrom",
                table: "PaymentModels");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "PaymentModels");

            migrationBuilder.DropColumn(
                name: "StatusChangeDate",
                table: "PaymentModels");
        }
    }
}
