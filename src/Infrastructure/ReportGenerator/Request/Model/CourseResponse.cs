using System.Diagnostics.CodeAnalysis;

namespace AACSB.WebApi.Infrastructure.ReportGenerator.Request.Model;

public class CourseResponse
{
    [AllowNull]
    public string Semester { get; set; } // 1111
    [AllowNull]
    public string CourseNo { get; set; } // MB112A001
    [AllowNull]
    public string CourseName { get; set; } // 經濟學(上)
    [AllowNull]
    public string CourseTeacher { get; set; } // 莊育娟
    [AllowNull]
    public string Dimension { get; set; } // ""
    [AllowNull]
    public string CreditPoint { get; set; } // 3
    [AllowNull]
    public string RequireOption { get; set; } // Required = R, Not required = E
    [AllowNull]
    public string AllYear { get; set; } // Full Year = F, Half Year = H
    [AllowNull]
    public int ChooseStudent { get; set; } // 42
    [AllowNull]
    public string Restrict1 { get; set; } // 9999
    [AllowNull]
    public string Restrict2 { get; set; } // 55
    [AllowNull]
    public int ThreeStudent { get; set; } // 0
    [AllowNull]
    public int AllStudent { get; set; } // 42
    [AllowNull]
    public string NTURestrict { get; set; } // 0
    [AllowNull]
    public string NTNURestrict { get; set; } // 0
    [AllowNull]
    public string CourseTimes { get; set; } // 3
    [AllowNull]
    public string PracticalTimes { get; set; } // 0
    [AllowNull]
    public string ClassRoomNo { get; set; } // IB-608
    [AllowNull]
    public string? ThreeNode { get; set; } // null
    [AllowNull]
    public string Node { get; set; } // W2,W3,W4
    [AllowNull]
    public string Contents { get; set; } // ""
    [AllowNull]
    public int NTU_People { get; set; } // 0
    [AllowNull]
    public int NTNU_People { get; set; } // 0
    [AllowNull]
    public int AbroadPeople { get; set; } // 0
}