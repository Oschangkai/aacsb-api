using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Add_Teacher_Related_Information : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "DepartmentAbbr",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Responsibility",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true,
                comment: "Email");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true,
                comment: "職稱");

            migrationBuilder.AddColumn<string>(
                name: "ClassRoomNo",
                schema: "ReportGenerator",
                table: "Courses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "教室");

            migrationBuilder.AddColumn<string>(
                name: "Contents",
                schema: "ReportGenerator",
                table: "Courses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "課程說明");

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "ReportGenerator",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Abbreviation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeacherProfessionals",
                schema: "ReportGenerator",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherProfessionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherProfessionals_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "ReportGenerator",
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherQualifications",
                schema: "ReportGenerator",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    EnglishDescription = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Abbreviation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherQualifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeacherResearch",
                schema: "ReportGenerator",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherResearch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherResearch_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "ReportGenerator",
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherResponsibilities",
                schema: "ReportGenerator",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    EnglishDescription = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Abbreviation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherResponsibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherResponsibilities_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "ReportGenerator",
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DepartmentId",
                schema: "ReportGenerator",
                table: "Teachers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherProfessionals_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherResearch_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherResponsibilities_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Departments_DepartmentId",
                schema: "ReportGenerator",
                table: "Teachers",
                column: "DepartmentId",
                principalSchema: "ReportGenerator",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Departments_DepartmentId",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "ReportGenerator");

            migrationBuilder.DropTable(
                name: "TeacherProfessionals",
                schema: "ReportGenerator");

            migrationBuilder.DropTable(
                name: "TeacherQualifications",
                schema: "ReportGenerator");

            migrationBuilder.DropTable(
                name: "TeacherResearch",
                schema: "ReportGenerator");

            migrationBuilder.DropTable(
                name: "TeacherResponsibilities",
                schema: "ReportGenerator");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_DepartmentId",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "ClassRoomNo",
                schema: "ReportGenerator",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Contents",
                schema: "ReportGenerator",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentAbbr",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                comment: "IM/FM/...，單一值");

            migrationBuilder.AddColumn<string>(
                name: "Responsibility",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "MT/RES/... 可以為多值，以逗點分割");
        }
    }
}
