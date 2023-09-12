using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Fix_Duplicated_Teacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LinkTo",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "uniqueidentifier",
                nullable: true,
                comment: "去重用，標示是否為同一人");

            const string view =
                "CREATE OR ALTER VIEW [ReportGenerator].[V_Table_A31_Course] AS\n" +
                "WITH CTE AS (\n" +
                "SELECT\n" +
                    "c.[Name] AS Course," +
                    "c.[Id] AS CourseId," +
                    "c.[Code] AS CourseCode," +
                    "c.[EnglishName] AS CourseEnglish," +
                    "c.[Time] AS CourseTime," +
                    "t.[Name] AS Teacher," +
                    "t.[Id] AS TeacherId," +
                    "t.[EnglishName] AS TeacherEnglishName," +
                    "d.[Name] AS CourseDepartment," +
                    "d.[Id] AS CourseDepartmentId," +
                    "t.[degree] AS Degree," +
                    "t.[DegreeYear] AS DegreeYear," +
                    "q.[Abbreviation] AS Qualification," +
                    "q.[Id] AS QualificationId," +
                    "t.[WorkTypeAbbr] AS WorkType," +
                    "di.[Code] AS Discipline," +
                    "di.[Id] AS DisciplineId," +
                    "c.[Semester]," +
                    "c.[Credit]," +
                    "COUNT(t.[Id]) OVER (PARTITION BY c.[Id]) AS TeacherCount," +
                    "ROW_NUMBER() OVER (PARTITION BY c.[Semester], ct.[TeacherId], c.[Time], c.[Name] ORDER BY CASE WHEN t.[DepartmentId] = c.[DepartmentId] THEN 0 ELSE 1 END) AS RowNum\n" +
                "FROM [ReportGenerator].[Courses] c\n" +
                "LEFT JOIN [ReportGenerator].[CourseTeacher] ct ON c.[Id] = ct.[CourseId]\n" +
                "LEFT JOIN [ReportGenerator].[Teachers] t ON\n" +
                    "CASE WHEN t.[LinkTo] IS NOT NULL THEN t.[LinkTo] ELSE t.[Id] END = ct.[TeacherId]\n" +
                "LEFT JOIN [ReportGenerator].[Discipline] di ON c.DisciplineId = di.Id\n" +
                "LEFT JOIN [ReportGenerator].[Qualifications] q ON t.QualificationId = q.Id\n" +
                "LEFT JOIN [ReportGenerator].[Departments] d ON c.[DepartmentId] = d.Id\n" +
                "WHERE t.Name <> ''\n" +
                    "AND c.Code NOT LIKE 'MIG%'\n" +
                    "AND c.Code NOT LIKE 'MBG%'\n" +
                    "AND c.Name NOT LIKE N'%專題%'\n" +
                    "AND c.Name NOT LIKE N'%實務專題%'\n" +
                    "AND c.Name NOT LIKE N'%管理實務%'\n" +
                    "AND c.Name NOT LIKE N'%經營實務%'\n" +
                    "AND c.Name NOT LIKE N'%書報研討%'\n" +
                    "AND c.Name NOT LIKE N'%企業研習%'\n" +
                    "AND c.Name NOT LIKE N'%實習%'\n" +
                    "AND c.Name NOT LIKE N'%講座%'\n" +
                    "AND c.Name NOT LIKE N'%論文導讀%'\n" +
                    "AND c.Name NOT LIKE N'%科技法規總論%'\n" +
                    "AND c.Name NOT LIKE N'%設計驅動創新實務%'\n" +
                    "AND c.Time IS NOT NULL\n" +
                    "AND c.Credit > 0\n" +
                    "AND (c.Credit % 1) = 0\n" +
                    "AND c.DeletedOn IS NULL\n" +
                ")" +
                "SELECT\n" +
                    "Course," +
                    "CourseEnglish," +
                    "CourseId," +
                    "CourseCode," +
                    "CourseTime," +
                    "Teacher," +
                    "TeacherId," +
                    "TeacherEnglishName," +
                    "CourseDepartment," +
                    "CourseDepartmentId," +
                    "Degree," +
                    "DegreeYear," +
                    "Qualification," +
                    "QualificationId," +
                    "WorkType," +
                    "Discipline," +
                    "DisciplineId," +
                    "Semester," +
                    "IIF(TeacherCount > 1, Credit / TeacherCount, Credit) AS Credit\n" +
                "FROM CTE\n" +
                "WHERE RowNum = 1;";
            migrationBuilder.Sql(view);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkTo",
                schema: "ReportGenerator",
                table: "Teachers");

            const string view =
                "CREATE OR ALTER VIEW [ReportGenerator].[V_Table_A31_Course] AS\n" +
                "WITH CTE AS (\n" +
                "SELECT\n" +
                    "c.[Name] AS Course," +
                    "c.[Id] AS CourseId," +
                    "c.[Code] AS CourseCode," +
                    "c.[EnglishName] AS CourseEnglish," +
                    "c.[Time] AS CourseTime," +
                    "t.[Name] AS Teacher," +
                    "t.[Id] AS TeacherId," +
                    "t.[EnglishName] AS TeacherEnglishName," +
                    "d.[Name] AS CourseDepartment," +
                    "d.[Id] AS CourseDepartmentId," +
                    "t.[degree] AS Degree," +
                    "t.[DegreeYear] AS DegreeYear," +
                    "q.[Abbreviation] AS Qualification," +
                    "q.[Id] AS QualificationId," +
                    "t.[WorkTypeAbbr] AS WorkType," +
                    "di.[Code] AS Discipline," +
                    "di.[Id] AS DisciplineId," +
                    "c.[Semester]," +
                    "c.[Credit]," +
                    "COUNT(t.[Id]) OVER (PARTITION BY c.[Id]) AS TeacherCount," +
                    "ROW_NUMBER() OVER (PARTITION BY c.[Semester], ct.[TeacherId], c.[Time], c.[Name] ORDER BY CASE WHEN t.[DepartmentId] = c.[DepartmentId] THEN 0 ELSE 1 END) AS RowNum\n" +
                "FROM [ReportGenerator].[Courses] c\n" +
                "LEFT JOIN [ReportGenerator].[CourseTeacher] ct ON c.[Id] = ct.[CourseId]\n" +
                "LEFT JOIN [ReportGenerator].[Teachers] t ON t.Id = ct.[TeacherId]\n" +
                "LEFT JOIN [ReportGenerator].[Discipline] di ON c.DisciplineId = di.Id\n" +
                "LEFT JOIN [ReportGenerator].[Qualifications] q ON t.QualificationId = q.Id\n" +
                "LEFT JOIN [ReportGenerator].[Departments] d ON c.[DepartmentId] = d.Id\n" +
                "WHERE t.Name <> ''\n" +
                    "AND c.Name NOT LIKE N'%專題%'\n" +
                    "AND c.Name NOT LIKE N'%實務專題%'\n" +
                    "AND c.Name NOT LIKE N'%管理實務%'\n" +
                    "AND c.Name NOT LIKE N'%經營實務%'\n" +
                    "AND c.Name NOT LIKE N'%書報研討%'\n" +
                    "AND c.Name NOT LIKE N'%企業研習%'\n" +
                    "AND c.Name NOT LIKE N'%實習%'\n" +
                    "AND c.Name NOT LIKE N'%講座%'\n" +
                    "AND c.Name NOT LIKE N'%論文導讀%'\n" +
                    "AND c.Name NOT LIKE N'%科技法規總論%'\n" +
                    "AND c.Name NOT LIKE N'%設計驅動創新實務%'\n" +
                    "AND c.Time IS NOT NULL\n" +
                    "AND c.Credit > 0\n" +
                    "AND (c.Credit % 1) = 0\n" +
                    "AND c.DeletedOn IS NULL\n" +
                ")" +
                "SELECT\n" +
                    "Course," +
                    "CourseEnglish," +
                    "CourseId," +
                    "CourseCode," +
                    "CourseTime," +
                    "Teacher," +
                    "TeacherId," +
                    "TeacherEnglishName," +
                    "CourseDepartment," +
                    "CourseDepartmentId," +
                    "Degree," +
                    "DegreeYear," +
                    "Qualification," +
                    "QualificationId," +
                    "WorkType," +
                    "Discipline," +
                    "DisciplineId," +
                    "Semester," +
                    "IIF(TeacherCount > 1, Credit / TeacherCount, Credit) AS Credit\n" +
                "FROM CTE\n" +
                "WHERE RowNum = 1;";
            migrationBuilder.Sql(view);

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
