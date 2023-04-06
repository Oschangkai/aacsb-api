using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Add_Course_Department_and_Fix_Teacher_Related_Relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherProfessionals_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherProfessionals");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherResearch_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherResearch");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherResponsibilities_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities");

            migrationBuilder.DropColumn(
                name: "Qualification",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities");

            migrationBuilder.DropColumn(
                name: "EnglishDescription",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ReportGenerator",
                table: "TeacherResearch");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "ReportGenerator",
                table: "TeacherResearch");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "ReportGenerator",
                table: "TeacherResearch");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "ReportGenerator",
                table: "TeacherResearch");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "ReportGenerator",
                table: "TeacherResearch");

            migrationBuilder.DropColumn(
                name: "EnglishDescription",
                schema: "ReportGenerator",
                table: "TeacherResearch");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                schema: "ReportGenerator",
                table: "TeacherResearch");

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                schema: "ReportGenerator",
                table: "TeacherQualifications");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ReportGenerator",
                table: "TeacherQualifications");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "ReportGenerator",
                table: "TeacherQualifications");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "ReportGenerator",
                table: "TeacherQualifications");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "ReportGenerator",
                table: "TeacherQualifications");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "ReportGenerator",
                table: "TeacherQualifications");

            migrationBuilder.DropColumn(
                name: "EnglishDescription",
                schema: "ReportGenerator",
                table: "TeacherQualifications");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                schema: "ReportGenerator",
                table: "TeacherQualifications");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "ReportGenerator",
                table: "TeacherProfessionals");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "ReportGenerator",
                table: "TeacherProfessionals");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "ReportGenerator",
                table: "TeacherProfessionals");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "ReportGenerator",
                table: "TeacherProfessionals");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "ReportGenerator",
                table: "TeacherProfessionals");

            migrationBuilder.DropColumn(
                name: "EnglishDescription",
                schema: "ReportGenerator",
                table: "TeacherProfessionals");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                schema: "ReportGenerator",
                table: "TeacherProfessionals");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                newName: "ResponsibilityId");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                newName: "ResearchId");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                newName: "QualificationId");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                newName: "ProfessionalId");

            migrationBuilder.AddColumn<Guid>(
                name: "QualificationId",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                schema: "ReportGenerator",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "_Professionals",
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
                    table.PrimaryKey("PK__Professionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Professionals_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "ReportGenerator",
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "_Qualifications",
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
                    table.PrimaryKey("PK__Qualifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "_Research",
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
                    table.PrimaryKey("PK__Research", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Research_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "ReportGenerator",
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "_Responsibilities",
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
                    table.PrimaryKey("PK__Responsibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Responsibilities_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "ReportGenerator",
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_QualificationId",
                schema: "ReportGenerator",
                table: "Teachers",
                column: "QualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherResponsibilities_ResponsibilityId",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                column: "ResponsibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherResearch_ResearchId",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                column: "ResearchId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherQualifications_QualificationId",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                column: "QualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherQualifications_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherProfessionals_ProfessionalId",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DepartmentId",
                schema: "ReportGenerator",
                table: "Courses",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX__Professionals_TeacherId",
                schema: "ReportGenerator",
                table: "_Professionals",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX__Research_TeacherId",
                schema: "ReportGenerator",
                table: "_Research",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX__Responsibilities_TeacherId",
                schema: "ReportGenerator",
                table: "_Responsibilities",
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
                name: "FK_TeacherProfessionals__Professionals_ProfessionalId",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                column: "ProfessionalId",
                principalSchema: "ReportGenerator",
                principalTable: "_Professionals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherProfessionals_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                column: "TeacherId",
                principalSchema: "ReportGenerator",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherQualifications__Qualifications_QualificationId",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                column: "QualificationId",
                principalSchema: "ReportGenerator",
                principalTable: "_Qualifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherQualifications_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                column: "TeacherId",
                principalSchema: "ReportGenerator",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherResearch__Research_ResearchId",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                column: "ResearchId",
                principalSchema: "ReportGenerator",
                principalTable: "_Research",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherResearch_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                column: "TeacherId",
                principalSchema: "ReportGenerator",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherResponsibilities__Responsibilities_ResponsibilityId",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                column: "ResponsibilityId",
                principalSchema: "ReportGenerator",
                principalTable: "_Responsibilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherResponsibilities_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                column: "TeacherId",
                principalSchema: "ReportGenerator",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers__Qualifications_QualificationId",
                schema: "ReportGenerator",
                table: "Teachers",
                column: "QualificationId",
                principalSchema: "ReportGenerator",
                principalTable: "_Qualifications",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                schema: "ReportGenerator",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherProfessionals__Professionals_ProfessionalId",
                schema: "ReportGenerator",
                table: "TeacherProfessionals");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherProfessionals_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherProfessionals");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherQualifications__Qualifications_QualificationId",
                schema: "ReportGenerator",
                table: "TeacherQualifications");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherQualifications_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherQualifications");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherResearch__Research_ResearchId",
                schema: "ReportGenerator",
                table: "TeacherResearch");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherResearch_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherResearch");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherResponsibilities__Responsibilities_ResponsibilityId",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherResponsibilities_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers__Qualifications_QualificationId",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropTable(
                name: "_Professionals",
                schema: "ReportGenerator");

            migrationBuilder.DropTable(
                name: "_Qualifications",
                schema: "ReportGenerator");

            migrationBuilder.DropTable(
                name: "_Research",
                schema: "ReportGenerator");

            migrationBuilder.DropTable(
                name: "_Responsibilities",
                schema: "ReportGenerator");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_QualificationId",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_TeacherResponsibilities_ResponsibilityId",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities");

            migrationBuilder.DropIndex(
                name: "IX_TeacherResearch_ResearchId",
                schema: "ReportGenerator",
                table: "TeacherResearch");

            migrationBuilder.DropIndex(
                name: "IX_TeacherQualifications_QualificationId",
                schema: "ReportGenerator",
                table: "TeacherQualifications");

            migrationBuilder.DropIndex(
                name: "IX_TeacherQualifications_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherQualifications");

            migrationBuilder.DropIndex(
                name: "IX_TeacherProfessionals_ProfessionalId",
                schema: "ReportGenerator",
                table: "TeacherProfessionals");

            migrationBuilder.DropIndex(
                name: "IX_Courses_DepartmentId",
                schema: "ReportGenerator",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "QualificationId",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                schema: "ReportGenerator",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "ResponsibilityId",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "ResearchId",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "QualificationId",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "ProfessionalId",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                newName: "LastModifiedBy");

            migrationBuilder.AddColumn<string>(
                name: "Qualification",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                comment: "SA/IP/...，單一值");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishDescription",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishDescription",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishDescription",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                schema: "ReportGenerator",
                table: "TeacherQualifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnglishDescription",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherProfessionals_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherProfessionals",
                column: "TeacherId",
                principalSchema: "ReportGenerator",
                principalTable: "Teachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherResearch_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherResearch",
                column: "TeacherId",
                principalSchema: "ReportGenerator",
                principalTable: "Teachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherResponsibilities_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "TeacherResponsibilities",
                column: "TeacherId",
                principalSchema: "ReportGenerator",
                principalTable: "Teachers",
                principalColumn: "Id");
        }
    }
}
