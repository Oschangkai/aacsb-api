using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Modify_Responsibilities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Responsibilities",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.AddColumn<bool>(
                name: "Supervisor",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "bit",
                nullable: true,
                comment: "是否擔任主管");

            const string view =
                "CREATE OR ALTER VIEW [ReportGenerator].[V_Table_A31_Course] AS\n" +
                "WITH CTE AS (\n" +
                "SELECT\n" +
                    "c.[Name] AS Course," +
                    "c.[Id] AS CourseId," +
                    "c.[Code] AS CourseCode," +
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

            const string func2 =
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
            migrationBuilder.Sql(func2);

            const string func =
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
                                "WHEN c.[WorkType] = 'S' THEN 'SER'\n" +
                                "ELSE NULL END,\n" +
                            "CASE\n" +
                                "WHEN t.[Supervisor] = 1 THEN 'ADM'\n" +
                                "ELSE NULL END\n" +
                        ") AS Responsibilities\n" +
                    "FROM [ReportGenerator].[V_Table_A31_Course] c\n" +
                    "LEFT JOIN [ReportGenerator].[Teachers] t ON t.[Id] = c.[TeacherId]\n" +
                    "WHERE ([Semester] = @SemesterYear+'1' OR [Semester] = @SemesterYear+'2')\n" +
                    "GROUP BY c.[TeacherId], c.[WorkType], t.[Supervisor]";
            migrationBuilder.Sql(func);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Supervisor",
                schema: "ReportGenerator",
                table: "Teachers");

            migrationBuilder.AddColumn<string>(
                name: "Responsibilities",
                schema: "ReportGenerator",
                table: "Teachers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "MT/RES/... 可以為多值");

            const string view =
                "CREATE OR ALTER VIEW [ReportGenerator].[V_Table_A31_Course] AS\n" +
                "WITH CTE AS (\n" +
                "SELECT\n" +
                    "c.[Name] AS Course," +
                    "c.[Id] AS CourseId," +
                    "c.[Code] AS CourseCode," +
                    "c.[Time] AS CourseTime," +
                    "t.[Name] AS Teacher," +
                    "t.[Id] AS TeacherId," +
                    "t.[EnglishName] AS TeacherEnglishName," +
                    "d.[Name] AS CourseDepartment," +
                    "d.[Id] AS CourseDepartmentId," +
                    "t.[degree] AS Degree," +
                    "t.[DegreeYear] AS DegreeYear," +
                    "t.[Responsibilities] AS Responsibilities," +
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
                    "Responsibilities," +
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

            const string func2 =
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
                        "c.[Responsibilities]," +
                        "c.[Qualification]," +
                        "c.[WorkType]," +
                        "c.[Discipline]" +
                    "FROM [ReportGenerator].[V_Table_A31_Course] c\n" +
                    "LEFT JOIN [ReportGenerator].[Teachers] t ON c.[TeacherId] = t.[Id]\n" +
                    "LEFT JOIN [ReportGenerator].[Departments] d ON t.[DepartmentId] = d.[Id]\n" +
                    "WHERE\n" +
                        "([Semester] = @SemesterYear+'1' OR [Semester] = @SemesterYear+'2')";
            migrationBuilder.Sql(func2);

            const string func = "DROP FUNCTION [ReportGenerator].[F_GetTeacherResponsibilities]";
            migrationBuilder.Sql(func);
        }
    }
}
