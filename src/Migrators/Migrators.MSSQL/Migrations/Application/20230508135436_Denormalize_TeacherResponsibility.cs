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
