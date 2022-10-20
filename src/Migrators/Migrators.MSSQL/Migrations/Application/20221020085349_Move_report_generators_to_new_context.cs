using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Move_report_generators_to_new_context : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Disciplines_DisciplineId",
                schema: "Catalog",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "Disciplines",
                schema: "Catalog");

            migrationBuilder.EnsureSchema(
                name: "ReportGenerator");

            migrationBuilder.RenameTable(
                name: "Teachers",
                schema: "Catalog",
                newName: "Teachers",
                newSchema: "ReportGenerator");

            migrationBuilder.RenameTable(
                name: "ImportSignatures",
                schema: "Catalog",
                newName: "ImportSignatures",
                newSchema: "ReportGenerator");

            migrationBuilder.RenameTable(
                name: "CourseTeacher",
                schema: "Catalog",
                newName: "CourseTeacher",
                newSchema: "ReportGenerator");

            migrationBuilder.RenameTable(
                name: "Courses",
                schema: "Catalog",
                newName: "Courses",
                newSchema: "ReportGenerator");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "ReportGenerator",
                table: "ImportSignatures",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "ReportGenerator",
                table: "Courses",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.CreateTable(
                name: "Discipline",
                schema: "ReportGenerator",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<decimal>(type: "decimal(2,0)", precision: 2, scale: 0, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discipline", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Discipline_DisciplineId",
                schema: "ReportGenerator",
                table: "Courses",
                column: "DisciplineId",
                principalSchema: "ReportGenerator",
                principalTable: "Discipline",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Discipline_DisciplineId",
                schema: "ReportGenerator",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "Discipline",
                schema: "ReportGenerator");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "ReportGenerator",
                table: "ImportSignatures");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "ReportGenerator",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "Teachers",
                schema: "ReportGenerator",
                newName: "Teachers",
                newSchema: "Catalog");

            migrationBuilder.RenameTable(
                name: "ImportSignatures",
                schema: "ReportGenerator",
                newName: "ImportSignatures",
                newSchema: "Catalog");

            migrationBuilder.RenameTable(
                name: "CourseTeacher",
                schema: "ReportGenerator",
                newName: "CourseTeacher",
                newSchema: "Catalog");

            migrationBuilder.RenameTable(
                name: "Courses",
                schema: "ReportGenerator",
                newName: "Courses",
                newSchema: "Catalog");

            migrationBuilder.CreateTable(
                name: "Disciplines",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<decimal>(type: "decimal(2,0)", precision: 2, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplines", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Disciplines_DisciplineId",
                schema: "Catalog",
                table: "Courses",
                column: "DisciplineId",
                principalSchema: "Catalog",
                principalTable: "Disciplines",
                principalColumn: "Id");
        }
    }
}
