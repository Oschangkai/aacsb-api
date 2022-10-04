using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Add_Course_and_Teacher_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Identity",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "Group",
                schema: "Identity",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "Identity",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                schema: "Identity",
                table: "RoleClaims");

            migrationBuilder.CreateTable(
                name: "Disciplines",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<decimal>(type: "decimal(2,0)", precision: 2, scale: 0, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportSignatures",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportSignatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Qualification = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "SA/IP/...，單一值"),
                    DepartmentAbbr = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "IM/FM/...，單一值"),
                    Type = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    TypeAbbr = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, comment: "P=Full Time, F=Part Time, C=Contractual"),
                    EnglishName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Degree = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DegreeYear = table.Column<decimal>(type: "decimal(4,0)", precision: 4, scale: 0, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Responsibility = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "MT/RES/... 可以為多值，以逗點分割"),
                    ImportSignatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_ImportSignatures_ImportSignatureId",
                        column: x => x.ImportSignatureId,
                        principalSchema: "Catalog",
                        principalTable: "ImportSignatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisciplineCode = table.Column<decimal>(type: "decimal(2,0)", precision: 2, scale: 0, nullable: true),
                    Semester = table.Column<decimal>(type: "decimal(5,0)", precision: 5, scale: 0, nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CourseNameEng = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TeacherName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    TeacherEnglishName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CourseCredit = table.Column<decimal>(type: "decimal(6,4)", precision: 6, scale: 4, nullable: false),
                    CourseRequired = table.Column<bool>(type: "bit", nullable: false, comment: "必修/選修"),
                    CourseYear = table.Column<bool>(type: "bit", nullable: false, comment: "全半學年"),
                    CourseTime = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "節次，M1, T6, W3，以逗點分隔"),
                    ImportSignatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisciplineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalSchema: "Catalog",
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_ImportSignatures_ImportSignatureId",
                        column: x => x.ImportSignatureId,
                        principalSchema: "Catalog",
                        principalTable: "ImportSignatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "Catalog",
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DisciplineId",
                schema: "Catalog",
                table: "Courses",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ImportSignatureId",
                schema: "Catalog",
                table: "Courses",
                column: "ImportSignatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                schema: "Catalog",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_ImportSignatureId",
                schema: "Catalog",
                table: "Teachers",
                column: "ImportSignatureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "Disciplines",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "Teachers",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "ImportSignatures",
                schema: "Catalog");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Identity",
                table: "RoleClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                schema: "Identity",
                table: "RoleClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                schema: "Identity",
                table: "RoleClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                schema: "Identity",
                table: "RoleClaims",
                type: "datetime2",
                nullable: true);
        }
    }
}
