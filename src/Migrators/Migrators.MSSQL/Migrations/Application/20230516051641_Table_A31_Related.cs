using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Table_A31_Related : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string view =
                "CREATE OR ALTER VIEW [ReportGenerator].[V_Table_A31_Course] WITH SCHEMABINDING AS\n" +
                "WITH CTE AS (\n" +
                "SELECT\n" +
                    "c.[Name] AS Course," +
                    "c.[Id] AS CourseId," +
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
                    "AND c.Credit > 0\n" +
                    "AND (c.Credit % 1) = 0" +
                ")" +
                "SELECT\n" +
                    "Course," +
                    "CourseId," +
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
                    "CASE WHEN TeacherCount > 1 THEN Credit / TeacherCount ELSE Credit END AS Credit\n" +
                "FROM CTE\n" +
                "WHERE RowNum = 1;";
            const string func =
                "CREATE OR ALTER FUNCTION [ReportGenerator].[F_GetTeacherDiscipline](@SemesterYear nvarchar(3))\n" +
                "RETURNS TABLE\n" +
                    "AS RETURN\n" +
                    "SELECT DISTINCT\n" +
                        "SUM(c.[Credit]) OVER(PARTITION BY c.[Discipline], c.[Teacher]) AS [DisciplineTotal]," +
                        "c.[TeacherId]," +
                        "c.[Teacher]," +
                        "d.[EnglishName] AS [Department]," +
                        "c.[Degree] + ', ' + convert(varchar, c.[DegreeYear]) AS Degree," +
                        "c.[Responsibilities]," +
                        "q.[Abbreviation] AS [Qualification]," +
                        "c.[WorkType]," +
                        "c.[Discipline]" +
                    "FROM [ReportGenerator].[V_Table_A31_Course] c\n" +
                    "LEFT JOIN [ReportGenerator].[Qualifications] q ON c.[QualificationId] = q.[Id]\n" +
                    "LEFT JOIN [ReportGenerator].[Teachers] t ON c.[TeacherId] = t.[Id]\n" +
                    "LEFT JOIN [ReportGenerator].[Departments] d ON t.[DepartmentId] = d.[Id]\n" +
                    "WHERE\n" +
                        "([Semester] = @SemesterYear+'1' OR [Semester] = @SemesterYear+'2')";

            migrationBuilder.Sql(view);
            migrationBuilder.Sql(func);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string view = "DROP VIEW [ReportGenerator].[V_Table_A31_Course]";
            const string func = "DROP FUNCTION [ReportGenerator].[F_GetTeacherDiscipline]";

            migrationBuilder.Sql(view);
            migrationBuilder.Sql(func);
        }
    }
}
