using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Table_A32_Related : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string func =
                "CREATE OR ALTER FUNCTION [ReportGenerator].[F_GetQualificationPercentage](@Semester VARCHAR(4), @Type VARCHAR(10))\n" +
                "RETURNS TABLE\n" +
                "AS\n" +
                "RETURN\n" +
                "WITH QualificationPercentage AS (\n" +
                    "SELECT\n" +
                        "q.[Abbreviation] AS Qualification,\n" +
                        "COUNT(*) * 1.0 / SUM(COUNT(*)) OVER () AS Percentage\n" +
                    "FROM\n" +
                        "[ReportGenerator].[V_Table_A31_Course] c\n" +
                        "LEFT JOIN [ReportGenerator].[CourseTeacher] ct ON c.[CourseId] = ct.[CourseId]\n" +
                        "LEFT JOIN [ReportGenerator].[Teachers] t ON t.[Id] = ct.[TeacherId]\n" +
                        "LEFT JOIN [ReportGenerator].[Qualifications] q ON t.[QualificationId] = q.[Id]\n" +
                    "WHERE\n" +
                        "t.Name <> ''\n" +
                        "AND c.Credit > 0\n" +
                        "AND (c.Credit % 1) = 0\n" +
                        "AND (c.Semester = @Semester+'1' OR Semester = @Semester+'2')\n" +
                        "AND (\n" +
                            "(\n" +
                                "@Type = 'bachelor'\n" +
                                "AND SUBSTRING(c.CourseCode, 3, 1) IN ('1', '2', '3', '4')\n" +
                                "AND SUBSTRING(c.CourseCode, 1, 2) IN ('IM', 'BA', 'MI', 'MB')\n" +
                            ")\n" +
                           "OR (\n" +
                                "@Type = 'master'\n" +
                                "AND SUBSTRING(c.CourseCode, 3, 1) IN ('5', '6', '7', '8')\n" +
                                "AND SUBSTRING(c.CourseCode, 1, 2) IN ('IM', 'BA', 'MI', 'FN', 'TM', 'MA')\n" +
                            ")\n" +
                           "OR (\n" +
                                "@Type = 'mba' AND (\n" +
                                "c.CourseCode LIKE '9%'\n" +
                                "OR c.CourseCode LIKE 'MG%'\n" +
                                "OR EXISTS (\n" +
                                    "SELECT 1\n" +
                                    "FROM STRING_SPLIT(c.CourseTime, ',')\n" +
                                    "WHERE value LIKE 'S%'\n" +
                                ")\n" +
                                "OR EXISTS (\n" +
                                    "SELECT 1\n" +
                                    "FROM STRING_SPLIT(c.CourseTime, ',')\n" +
                                    "WHERE value LIKE '%[ABC]'\n" +
                                ")\n" +
                            "))\n" +
                        ")\n" +
                    "GROUP BY\n" +
                        "q.[Abbreviation]\n" +
                ")\n" +
                "SELECT\n" +
                    "qp.Qualification,\n" +
                    "CAST(ROUND(qp.Percentage, 3) AS DECIMAL(2,2)) AS Percentage\n" +
                "FROM\n" +
                    "QualificationPercentage qp;";
            migrationBuilder.Sql(func);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string func = "DROP FUNCTION [ReportGenerator].[F_GetQualificationPercentage]";
            migrationBuilder.Sql(func);
        }
    }
}
