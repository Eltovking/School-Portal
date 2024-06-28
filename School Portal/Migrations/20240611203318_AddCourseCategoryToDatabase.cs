using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Portal.Migrations
{
    public partial class AddCourseCategoryToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseCategory_CourseCategoryId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseCategory",
                table: "CourseCategory");

            migrationBuilder.RenameTable(
                name: "CourseCategory",
                newName: "CourseCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseCategories",
                table: "CourseCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseCategories_CourseCategoryId",
                table: "Courses",
                column: "CourseCategoryId",
                principalTable: "CourseCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseCategories_CourseCategoryId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseCategories",
                table: "CourseCategories");

            migrationBuilder.RenameTable(
                name: "CourseCategories",
                newName: "CourseCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseCategory",
                table: "CourseCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseCategory_CourseCategoryId",
                table: "Courses",
                column: "CourseCategoryId",
                principalTable: "CourseCategory",
                principalColumn: "Id");
        }
    }
}
