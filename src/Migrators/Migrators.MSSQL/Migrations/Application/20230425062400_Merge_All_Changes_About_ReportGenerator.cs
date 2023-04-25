using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Merge_All_Changes_About_ReportGenerator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseTeacher_Courses_CoursesId",
                schema: "ReportGenerator",
                table: "CourseTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseTeacher_Teachers_TeachersId",
                schema: "ReportGenerator",
                table: "CourseTeacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseTeacher",
                schema: "ReportGenerator",
                table: "CourseTeacher");

            migrationBuilder.DropColumn(
                name: "Department",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "DepartmentAbbr",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Qualification",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Responsibility",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.RenameColumn(
                name: "TeachersId",
                schema: "ReportGenerator",
                table: "CourseTeacher",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "CoursesId",
                schema: "ReportGenerator",
                table: "CourseTeacher",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseTeacher_TeachersId",
                schema: "ReportGenerator",
                table: "CourseTeacher",
                newName: "IX_CourseTeacher_TeacherId");

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
                name: "EnglishNameInNtustCourse",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameInNtustCourse",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QualificationId",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResignDate",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "datetime2",
                nullable: true,
                comment: "離職日期");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true,
                comment: "職稱");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "ReportGenerator",
                table: "CourseTeacher",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                schema: "ReportGenerator",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseTeacher",
                schema: "ReportGenerator",
                table: "CourseTeacher",
                column: "Id");

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
                name: "Professionals",
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
                    table.PrimaryKey("PK_Professionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Professionals_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "ReportGenerator",
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Qualifications",
                schema: "ReportGenerator",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    EnglishDescription = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
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
                    table.PrimaryKey("PK_Qualifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Research",
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
                    table.PrimaryKey("PK_Research", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Research_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "ReportGenerator",
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Responsibilities",
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
                    table.PrimaryKey("PK_Responsibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responsibilities_Teachers_TeacherId",
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
                name: "IX_Teachers_QualificationId",
                schema: "ReportGenerator",
                table: "Teachers",
                column: "QualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTeacher_CourseId",
                schema: "ReportGenerator",
                table: "CourseTeacher",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DepartmentId",
                schema: "ReportGenerator",
                table: "Courses",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_TeacherId",
                schema: "ReportGenerator",
                table: "Professionals",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Research_TeacherId",
                schema: "ReportGenerator",
                table: "Research",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Responsibilities_TeacherId",
                schema: "ReportGenerator",
                table: "Responsibilities",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                schema: "ReportGenerator",
                table: "Courses",
                column: "DepartmentId",
                principalSchema: "ReportGenerator",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTeacher_Courses_CourseId",
                schema: "ReportGenerator",
                table: "CourseTeacher",
                column: "CourseId",
                principalSchema: "ReportGenerator",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTeacher_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "CourseTeacher",
                column: "TeacherId",
                principalSchema: "ReportGenerator",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Departments_DepartmentId",
                schema: "ReportGenerator",
                table: "Teachers",
                column: "DepartmentId",
                principalSchema: "ReportGenerator",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Qualifications_QualificationId",
                schema: "ReportGenerator",
                table: "Teachers",
                column: "QualificationId",
                principalSchema: "ReportGenerator",
                principalTable: "Qualifications",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                schema: "ReportGenerator",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseTeacher_Courses_CourseId",
                schema: "ReportGenerator",
                table: "CourseTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseTeacher_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "CourseTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Departments_DepartmentId",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Qualifications_QualificationId",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "ReportGenerator");

            migrationBuilder.DropTable(
                name: "Professionals",
                schema: "ReportGenerator");

            migrationBuilder.DropTable(
                name: "Qualifications",
                schema: "ReportGenerator");

            migrationBuilder.DropTable(
                name: "Research",
                schema: "ReportGenerator");

            migrationBuilder.DropTable(
                name: "Responsibilities",
                schema: "ReportGenerator");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_DepartmentId",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_QualificationId",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseTeacher",
                schema: "ReportGenerator",
                table: "CourseTeacher");

            migrationBuilder.DropIndex(
                name: "IX_CourseTeacher_CourseId",
                schema: "ReportGenerator",
                table: "CourseTeacher");

            migrationBuilder.DropIndex(
                name: "IX_Courses_DepartmentId",
                schema: "ReportGenerator",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "EnglishNameInNtustCourse",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "NameInNtustCourse",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "QualificationId",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "ResignDate",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "ReportGenerator",
                table: "CourseTeacher");

            migrationBuilder.DropColumn(
                name: "ClassRoomNo",
                schema: "ReportGenerator",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Contents",
                schema: "ReportGenerator",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                schema: "ReportGenerator",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                schema: "ReportGenerator",
                table: "CourseTeacher",
                newName: "TeachersId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                schema: "ReportGenerator",
                table: "CourseTeacher",
                newName: "CoursesId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseTeacher_TeacherId",
                schema: "ReportGenerator",
                table: "CourseTeacher",
                newName: "IX_CourseTeacher_TeachersId");

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
                defaultValue: string.Empty,
                comment: "IM/FM/...，單一值");

            migrationBuilder.AddColumn<string>(
                name: "Qualification",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: string.Empty,
                comment: "SA/IP/...，單一值");

            migrationBuilder.AddColumn<string>(
                name: "Responsibility",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "MT/RES/... 可以為多值，以逗點分割");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseTeacher",
                schema: "ReportGenerator",
                table: "CourseTeacher",
                columns: new[] { "CoursesId", "TeachersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTeacher_Courses_CoursesId",
                schema: "ReportGenerator",
                table: "CourseTeacher",
                column: "CoursesId",
                principalSchema: "ReportGenerator",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseTeacher_Teachers_TeachersId",
                schema: "ReportGenerator",
                table: "CourseTeacher",
                column: "TeachersId",
                principalSchema: "ReportGenerator",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
