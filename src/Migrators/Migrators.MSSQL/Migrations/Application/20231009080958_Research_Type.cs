using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Research_Type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResearchType",
                schema: "ReportGenerator",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Certification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResearchResearchType",
                schema: "ReportGenerator",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResearchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResearchTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchResearchType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchResearchType_Research_ResearchId",
                        column: x => x.ResearchId,
                        principalSchema: "ReportGenerator",
                        principalTable: "Research",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResearchResearchType_ResearchType_ResearchTypeId",
                        column: x => x.ResearchTypeId,
                        principalSchema: "ReportGenerator",
                        principalTable: "ResearchType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResearchResearchType_ResearchId",
                schema: "ReportGenerator",
                table: "ResearchResearchType",
                column: "ResearchId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchResearchType_ResearchTypeId",
                schema: "ReportGenerator",
                table: "ResearchResearchType",
                column: "ResearchTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResearchResearchType",
                schema: "ReportGenerator");

            migrationBuilder.DropTable(
                name: "ResearchType",
                schema: "ReportGenerator");
        }
    }
}
