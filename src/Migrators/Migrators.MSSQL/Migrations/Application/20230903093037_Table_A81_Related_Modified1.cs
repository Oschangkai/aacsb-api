using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Table_A81_Related_Modified1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Portfolio",
                schema: "ReportGenerator",
                table: "Research",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Applied-Integration",
                comment: "Basic-Discovery, Applied-Integration, Teaching-Learning");

            const string func =
                "CREATE OR ALTER FUNCTION [ReportGenerator].[F_GetTeacherResearchCount](@SemesterYear nvarchar(3))\n" +
                "RETURNS TABLE\n" +
                    "AS RETURN\n" +
                    "SELECT DISTINCT\n" +
                        "CAST(ROUND(SUM(c.[Credit]) OVER(PARTITION BY c.[Discipline], c.[Teacher]), 2) AS decimal(4,2)) AS [DisciplineTotal],\n" +
                        "CAST(ROUND(SUM(c.[Credit]) OVER(PARTITION BY c.[Teacher]), 2) AS decimal(4,1)) AS [CreditTotal],\n" +
                        "c.[Discipline],\n" +
                        "c.[TeacherId],c.Teacher,c.WorkType,\n" +
                        "rt.Journal1, rt.Journal2, rt.Others, rt.Basic, rt.Applied, rt.Teaching\n" +
                    "FROM [ReportGenerator].[V_Table_A31_Course] c\n" +
                        "LEFT JOIN (\n" +
                            "SELECT\n" +
                                "[TeacherId],\n" +
                                "COUNT(CASE WHEN [Type] = 'Journal 1' THEN 1 END) AS Journal1,\n" +
                                "COUNT(CASE WHEN [Type] = 'Journal 2' THEN 1 END) AS Journal2,\n" +
                                "COUNT(CASE WHEN [Type] IN ('Presentation', 'Proceeding') THEN 1 END) AS Others,\n" +
                                "COUNT(CASE WHEN [Portfolio] = 'Basic-Discovery' THEN 1 END) AS Basic,\n" +
                                "COUNT(CASE WHEN [Portfolio] = 'Applied-Integration' THEN 1 END) AS Applied,\n" +
                                "COUNT(CASE WHEN [Portfolio] = 'Teaching-Learning' THEN 1 END) AS Teaching\n" +
                            "FROM [ReportGenerator].[Research]\n" +
                            "GROUP BY [TeacherId]\n" +
                        ") rt ON c.[TeacherId] = rt.[TeacherId]\n" +
                    "WHERE [WorkType] IS NOT NULL\n" +
                    "AND [Semester] IN (@SemesterYear+'1', @SemesterYear+'2')";
            migrationBuilder.Sql(func);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string func = "DROP FUNCTION [ReportGenerator].[F_GetTeacherResearchCount]";
            migrationBuilder.Sql(func);

            migrationBuilder.DropColumn(
                name: "Portfolio",
                schema: "ReportGenerator",
                table: "Research");
        }
    }
}
