using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Denormalize_TeacherResponsibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responsibilities_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "Responsibilities");

            migrationBuilder.DropIndex(
                name: "IX_Responsibilities_TeacherId",
                schema: "ReportGenerator",
                table: "Responsibilities");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                schema: "ReportGenerator",
                table: "Responsibilities");

            migrationBuilder.AddColumn<string>(
                name: "Responsibilities",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "MT/RES/... 可以為多值");

            migrationBuilder.AlterColumn<bool>(
                name: "Year",
                schema: "ReportGenerator",
                table: "Courses",
                type: "bit",
                nullable: true,
                comment: "全半學年",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "全半學年");

            migrationBuilder.AlterColumn<string>(
                name: "Time",
                schema: "ReportGenerator",
                table: "Courses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "節次，M1, T6, W3，以逗點分隔",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldComment: "節次，M1, T6, W3，以逗點分隔");

            migrationBuilder.AlterColumn<bool>(
                name: "Required",
                schema: "ReportGenerator",
                table: "Courses",
                type: "bit",
                nullable: true,
                comment: "必修/選修",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "必修/選修");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Responsibilities",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                schema: "ReportGenerator",
                table: "Responsibilities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Year",
                schema: "ReportGenerator",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "全半學年",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldComment: "全半學年");

            migrationBuilder.AlterColumn<string>(
                name: "Time",
                schema: "ReportGenerator",
                table: "Courses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                comment: "節次，M1, T6, W3，以逗點分隔",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "節次，M1, T6, W3，以逗點分隔");

            migrationBuilder.AlterColumn<bool>(
                name: "Required",
                schema: "ReportGenerator",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "必修/選修",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldComment: "必修/選修");

            migrationBuilder.CreateIndex(
                name: "IX_Responsibilities_TeacherId",
                schema: "ReportGenerator",
                table: "Responsibilities",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Responsibilities_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "Responsibilities",
                column: "TeacherId",
                principalSchema: "ReportGenerator",
                principalTable: "Teachers",
                principalColumn: "Id");
        }
    }
}
