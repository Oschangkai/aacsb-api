using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Table_A81_Related : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Research_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.RenameColumn(
                name: "EnglishDescription",
                schema: "ReportGenerator",
                table: "Research",
                newName: "Seminar");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "ReportGenerator",
                table: "Research",
                newName: "OtherAuthors");

            migrationBuilder.AlterColumn<string>(
                name: "WorkTypeAbbr",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                comment: "P=Full Time, S=Part Time, C=Contractual",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true,
                oldComment: "P=Full Time, F=Part Time, C=Contractual");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                comment: "Email",
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80,
                oldNullable: true,
                oldComment: "Email");

            migrationBuilder.AlterColumn<short>(
                name: "DegreeYear",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,0)",
                oldPrecision: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "ReportGenerator",
                table: "Research",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AddressAuthors",
                schema: "ReportGenerator",
                table: "Research",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "ReportGenerator",
                table: "Research",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                schema: "ReportGenerator",
                table: "Research",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "DayEnd",
                schema: "ReportGenerator",
                table: "Research",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "DayStart",
                schema: "ReportGenerator",
                table: "Research",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FullText",
                schema: "ReportGenerator",
                table: "Research",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Issue",
                schema: "ReportGenerator",
                table: "Research",
                type: "nvarchar(10)",
                nullable: true,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "JournalsClass",
                schema: "ReportGenerator",
                table: "Research",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JournalsName",
                schema: "ReportGenerator",
                table: "Research",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JournalsStatus",
                schema: "ReportGenerator",
                table: "Research",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                comment: "已接受/已發表");

            migrationBuilder.AddColumn<string>(
                name: "JournalsType",
                schema: "ReportGenerator",
                table: "Research",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                comment: "電子期刊/紙本");

            migrationBuilder.AddColumn<byte>(
                name: "Month",
                schema: "ReportGenerator",
                table: "Research",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "MonthEnd",
                schema: "ReportGenerator",
                table: "Research",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "MonthStart",
                schema: "ReportGenerator",
                table: "Research",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderAuthors",
                schema: "ReportGenerator",
                table: "Research",
                type: "nvarchar(max)",
                nullable: true,
                comment: "第一作者/.../第四(以上)作者");

            migrationBuilder.AddColumn<short>(
                name: "PageEnd",
                schema: "ReportGenerator",
                table: "Research",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PageStart",
                schema: "ReportGenerator",
                table: "Research",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Project",
                schema: "ReportGenerator",
                table: "Research",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Publication",
                schema: "ReportGenerator",
                table: "Research",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "ReportGenerator",
                table: "Research",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "ReportGenerator",
                table: "Research",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: string.Empty,
                comment: "Journal 1, Journal 2, Presentation, Proceeding");

            migrationBuilder.AddColumn<short>(
                name: "Volume",
                schema: "ReportGenerator",
                table: "Research",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Year",
                schema: "ReportGenerator",
                table: "Research",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "YearEnd",
                schema: "ReportGenerator",
                table: "Research",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "YearStart",
                schema: "ReportGenerator",
                table: "Research",
                type: "smallint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "ReportGenerator",
                table: "Discipline",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<byte>(
                name: "Code",
                schema: "ReportGenerator",
                table: "Discipline",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,0)",
                oldPrecision: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Time",
                schema: "ReportGenerator",
                table: "Courses",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                comment: "節次，M1, T6, W3，以逗點分隔",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "節次，M1, T6, W3，以逗點分隔");

            migrationBuilder.AlterColumn<short>(
                name: "Semester",
                schema: "ReportGenerator",
                table: "Courses",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,0)",
                oldPrecision: 5);

            migrationBuilder.AddForeignKey(
                name: "FK_Research_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "Research",
                column: "TeacherId",
                principalSchema: "ReportGenerator",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Research_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "AddressAuthors",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "City",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "Country",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "DayEnd",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "DayStart",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "FullText",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "Issue",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "JournalsClass",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "JournalsName",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "JournalsStatus",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "JournalsType",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "Month",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "MonthEnd",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "MonthStart",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "OrderAuthors",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "PageEnd",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "PageStart",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "Project",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "Publication",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "Volume",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "Year",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "YearEnd",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "YearStart",
                schema: "ReportGenerator",
                table: "Research");

            migrationBuilder.RenameColumn(
                name: "Seminar",
                schema: "ReportGenerator",
                table: "Research",
                newName: "EnglishDescription");

            migrationBuilder.RenameColumn(
                name: "OtherAuthors",
                schema: "ReportGenerator",
                table: "Research",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "WorkTypeAbbr",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                comment: "P=Full Time, F=Part Time, C=Contractual",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true,
                oldComment: "P=Full Time, S=Part Time, C=Contractual");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true,
                comment: "Email",
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "Email");

            migrationBuilder.AlterColumn<decimal>(
                name: "DegreeYear",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "decimal(4,0)",
                precision: 4,
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                schema: "ReportGenerator",
                table: "Research",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "ReportGenerator",
                table: "Discipline",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<decimal>(
                name: "Code",
                schema: "ReportGenerator",
                table: "Discipline",
                type: "decimal(2,0)",
                precision: 2,
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "Time",
                schema: "ReportGenerator",
                table: "Courses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "節次，M1, T6, W3，以逗點分隔",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "節次，M1, T6, W3，以逗點分隔");

            migrationBuilder.AlterColumn<decimal>(
                name: "Semester",
                schema: "ReportGenerator",
                table: "Courses",
                type: "decimal(5,0)",
                precision: 5,
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddForeignKey(
                name: "FK_Research_Teachers_TeacherId",
                schema: "ReportGenerator",
                table: "Research",
                column: "TeacherId",
                principalSchema: "ReportGenerator",
                principalTable: "Teachers",
                principalColumn: "Id");
        }
    }
}
