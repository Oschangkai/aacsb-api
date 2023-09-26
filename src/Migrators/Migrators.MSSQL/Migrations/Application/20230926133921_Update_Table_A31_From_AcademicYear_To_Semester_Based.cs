using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Update_Functions_From_AcademicYear_To_Semester_Based : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string dFunc =
                "CREATE OR ALTER FUNCTION [ReportGenerator].[F_GetTeacherDiscipline](@Semester nvarchar(MAX))\n" +
                "RETURNS TABLE\n" +
                    "AS RETURN\n" +
                    "SELECT DISTINCT\n" +
                        "CAST(ROUND((SUM(c.[Credit]) OVER(PARTITION BY c.[Discipline], c.[Teacher])), 2) AS decimal(4,2)) AS [DisciplineTotal]," +
                        "CAST(ROUND((SUM(c.[Credit]) OVER(PARTITION BY c.[Teacher])), 2) AS decimal(4,1)) AS [CreditTotal]," +
                        "c.[TeacherId]," +
                        "c.[Teacher]," +
                        "c.[TeacherEnglishName]," +
                        "d.[EnglishName] AS [TeacherDepartment]," +
                        "c.[Degree]," +
                        "c.[DegreeYear]," +
                        "c.[Qualification]," +
                        "c.[WorkType]," +
                        "c.[Discipline]" +
                    "FROM [ReportGenerator].[V_Table_A31_Course] c\n" +
                    "LEFT JOIN [ReportGenerator].[Teachers] t ON c.[TeacherId] = t.[Id]\n" +
                    "LEFT JOIN [ReportGenerator].[Departments] d ON t.[DepartmentId] = d.[Id]\n" +
                    "WHERE\n" +
                        "[Semester] IN (SELECT value FROM STRING_SPLIT(@Semester, ','))";
            migrationBuilder.Sql(dFunc);

            const string rFunc =
                "CREATE OR ALTER FUNCTION [ReportGenerator].[F_GetTeacherResponsibilities](@Semester nvarchar(MAX))\n" +
                "RETURNS TABLE\n" +
                    "AS RETURN\n" +
                    "SELECT [TeacherId]\n" +
                        ", CONCAT_WS(',',\n" +
                            "CASE\n" +
                                "WHEN SUM(IIF(SUBSTRING([CourseCode], 3, 1) IN ('1', '2', '3', '4'), 1, 0)) > 0 THEN 'UG'\n" +
                                "ELSE NULL END,\n" +
                            "CASE\n" +
                                "WHEN SUM(IIF(SUBSTRING([CourseCode], 3, 1) IN ('5', '6', '7', '8'), 1, 0)) > 0 THEN 'MT'\n" +
                                "ELSE NULL END,\n" +
                            "CASE\n" +
                                "WHEN c.[WorkType] = 'P' THEN 'RES,SER'\n" +
                                "ELSE NULL END,\n" +
                            "CASE\n" +
                                "WHEN t.[Supervisor] = 1 THEN 'ADM'\n" +
                                "ELSE NULL END\n" +
                        ") AS Responsibilities\n" +
                    "FROM [ReportGenerator].[V_Table_A31_Course] c\n" +
                    "LEFT JOIN [ReportGenerator].[Teachers] t ON t.[Id] = c.[TeacherId]\n" +
                    "WHERE [Semester] IN (SELECT value FROM STRING_SPLIT(@Semester, ','))\n" +
                    "GROUP BY c.[TeacherId], c.[WorkType], t.[Supervisor]";
            migrationBuilder.Sql(rFunc);

            const string rcFunc =
                "CREATE OR ALTER FUNCTION [ReportGenerator].[F_GetTeacherResearchCount](@Semester nvarchar(MAX))\n" +
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
                    "AND [Semester] IN (SELECT value FROM STRING_SPLIT(@Semester, ','))";
            migrationBuilder.Sql(rcFunc);

            const string qFunc =
                "CREATE OR ALTER FUNCTION [ReportGenerator].[F_GetQualificationPercentage](@Semester VARCHAR(MAX), @Type VARCHAR(10), @CourseDepartmentId uniqueidentifier = NULL)\n" +
                "RETURNS TABLE\n" +
                "AS\n" +
                "RETURN\n" +
                "WITH QualificationPercentage AS (\n" +
                    "SELECT\n" +
                        "q.[Abbreviation] AS Qualification,\n" +
                        "COUNT(*) * 1.0 / SUM(COUNT(*)) OVER () AS Percentage\n" +
                    "FROM\n" +
                        "[ReportGenerator].[V_Table_A31_Course] c\n" +
                        "LEFT JOIN [ReportGenerator].[Teachers] t ON t.[Id] = c.[TeacherId]\n" +
                        "LEFT JOIN [ReportGenerator].[Qualifications] q ON t.[QualificationId] = q.[Id]\n" +
                    "WHERE\n" +
                        "t.Name <> ''\n" +
                        "AND c.Credit > 0\n" +
                        "AND (c.Credit % 1) = 0\n" +
                        "AND [Semester] IN (SELECT value FROM STRING_SPLIT(@Semester, ','))\n" +
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
                        "AND (\n" +
                            "@CourseDepartmentId IS NULL\n" +
                            "OR c.CourseDepartmentId = @CourseDepartmentId\n" +
                        ")" +
                    "GROUP BY\n" +
                        "q.[Abbreviation]\n" +
                ")\n" +
                "SELECT\n" +
                    "qp.Qualification,\n" +
                    "CAST(ROUND(qp.Percentage, 3) AS DECIMAL(3,2)) AS Percentage\n" +
                "FROM\n" +
                    "QualificationPercentage qp;";
            migrationBuilder.Sql(qFunc);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string dFunc =
                "CREATE OR ALTER FUNCTION [ReportGenerator].[F_GetTeacherDiscipline](@SemesterYear nvarchar(3))\n" +
                "RETURNS TABLE\n" +
                    "AS RETURN\n" +
                    "SELECT DISTINCT\n" +
                        "CAST(ROUND((SUM(c.[Credit]) OVER(PARTITION BY c.[Discipline], c.[Teacher])), 2) AS decimal(4,2)) AS [DisciplineTotal]," +
                        "CAST(ROUND((SUM(c.[Credit]) OVER(PARTITION BY c.[Teacher])), 2) AS decimal(4,1)) AS [CreditTotal]," +
                        "c.[TeacherId]," +
                        "c.[Teacher]," +
                        "c.[TeacherEnglishName]," +
                        "d.[EnglishName] AS [TeacherDepartment]," +
                        "c.[Degree]," +
                        "c.[DegreeYear]," +
                        "c.[Qualification]," +
                        "c.[WorkType]," +
                        "c.[Discipline]" +
                    "FROM [ReportGenerator].[V_Table_A31_Course] c\n" +
                    "LEFT JOIN [ReportGenerator].[Teachers] t ON c.[TeacherId] = t.[Id]\n" +
                    "LEFT JOIN [ReportGenerator].[Departments] d ON t.[DepartmentId] = d.[Id]\n" +
                    "WHERE\n" +
                        "([Semester] = @SemesterYear+'1' OR [Semester] = @SemesterYear+'2')";
            migrationBuilder.Sql(dFunc);

            const string rFunc =
                "CREATE OR ALTER FUNCTION [ReportGenerator].[F_GetTeacherResponsibilities](@SemesterYear nvarchar(3))\n" +
                "RETURNS TABLE\n" +
                    "AS RETURN\n" +
                    "SELECT [TeacherId]\n" +
                        ", CONCAT_WS(',',\n" +
                            "CASE\n" +
                                "WHEN SUM(IIF(SUBSTRING([CourseCode], 3, 1) IN ('1', '2', '3', '4'), 1, 0)) > 0 THEN 'UG'\n" +
                                "ELSE NULL END,\n" +
                            "CASE\n" +
                                "WHEN SUM(IIF(SUBSTRING([CourseCode], 3, 1) IN ('5', '6', '7', '8'), 1, 0)) > 0 THEN 'MT'\n" +
                                "ELSE NULL END,\n" +
                            "CASE\n" +
                                "WHEN c.[WorkType] = 'P' THEN 'RES,SER'\n" +
                                "ELSE NULL END,\n" +
                            "CASE\n" +
                                "WHEN t.[Supervisor] = 1 THEN 'ADM'\n" +
                                "ELSE NULL END\n" +
                        ") AS Responsibilities\n" +
                    "FROM [ReportGenerator].[V_Table_A31_Course] c\n" +
                    "LEFT JOIN [ReportGenerator].[Teachers] t ON t.[Id] = c.[TeacherId]\n" +
                    "WHERE ([Semester] = @SemesterYear+'1' OR [Semester] = @SemesterYear+'2')\n" +
                    "GROUP BY c.[TeacherId], c.[WorkType], t.[Supervisor]";
            migrationBuilder.Sql(rFunc);

            const string rcFunc =
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
            migrationBuilder.Sql(rcFunc);

            const string qFunc =
                "CREATE OR ALTER FUNCTION [ReportGenerator].[F_GetQualificationPercentage](@Semester VARCHAR(4), @Type VARCHAR(10), @CourseDepartmentId uniqueidentifier = NULL)\n" +
                "RETURNS TABLE\n" +
                "AS\n" +
                "RETURN\n" +
                "WITH QualificationPercentage AS (\n" +
                    "SELECT\n" +
                        "q.[Abbreviation] AS Qualification,\n" +
                        "COUNT(*) * 1.0 / SUM(COUNT(*)) OVER () AS Percentage\n" +
                    "FROM\n" +
                        "[ReportGenerator].[V_Table_A31_Course] c\n" +
                        "LEFT JOIN [ReportGenerator].[Teachers] t ON t.[Id] = c.[TeacherId]\n" +
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
                        "AND (\n" +
                            "@CourseDepartmentId IS NULL\n" +
                            "OR c.CourseDepartmentId = @CourseDepartmentId\n" +
                        ")" +
                    "GROUP BY\n" +
                        "q.[Abbreviation]\n" +
                ")\n" +
                "SELECT\n" +
                    "qp.Qualification,\n" +
                    "CAST(ROUND(qp.Percentage, 3) AS DECIMAL(3,2)) AS Percentage\n" +
                "FROM\n" +
                    "QualificationPercentage qp;";
            migrationBuilder.Sql(qFunc);
        }
    }
}
