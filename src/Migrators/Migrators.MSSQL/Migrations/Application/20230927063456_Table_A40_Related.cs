using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrators.MSSQL.Migrations.Application
{
    public partial class Table_A40_Related : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    "(CASE\n" +
                        "WHEN (CourseCode LIKE 'MA%'\n" +
                                "AND NOT EXISTS(SELECT 1 FROM STRING_SPLIT(CourseTime, ',') WHERE value LIKE '[SU]%')\n" +
                                "AND NOT EXISTS(SELECT 1 FROM STRING_SPLIT(CourseTime, ',') WHERE value LIKE '%[ABCD]'))\n" +
                                "OR CourseDepartment = N'管理學院研究所'\n" +
                            "THEN 'MBA'\n" +
                        "WHEN CourseCode LIKE 'MG%'\n" +
                                "OR EXISTS (SELECT 1 FROM STRING_SPLIT(CourseTime, ',') WHERE value LIKE '[SU]%')\n" +
                                "OR EXISTS (SELECT 1 FROM STRING_SPLIT(CourseTime, ',') WHERE value LIKE '%[ABCD]')\n" +
                            "THEN 'EMBA'\n" +
                        "WHEN SUBSTRING(CourseCode, 3, 1) IN ('1', '2', '3', '4')\n" +
                                "AND SUBSTRING(CourseCode, 1, 2) IN ('IM', 'BA', 'MI', 'MB')\n" +
                            "THEN 'Bachelor'\n" +
                        "WHEN SUBSTRING(CourseCode, 3, 1) IN ('5', '6', '7')\n" +
                                "AND SUBSTRING(CourseCode, 1, 2) IN ('IM', 'BA', 'MI', 'FN', 'TM', 'MA')\n" +
                            "THEN 'Master'\n" +
                        "WHEN CourseCode LIKE '__9%'\n" +
                                "AND SUBSTRING(CourseCode, 1, 2) IN ('IM', 'BA', 'MI', 'FN', 'TM', 'MA')\n" +
                            "THEN 'PhD'\n" +
                        "ELSE NULL END) AS CourseDegree,\n" +
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
