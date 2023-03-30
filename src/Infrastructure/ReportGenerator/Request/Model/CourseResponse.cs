namespace AACSB.WebApi.Infrastructure.ReportGenerator.Request.Model;

public class CourseResponse
{
    public string Semester { get; set; } // 1111
    public string CourseNo { get; set; } // MB112A001
    public string CourseName { get; set; } // 經濟學(上)
    public string CourseTeacher { get; set; } // 莊育娟
    public string Dimension { get; set; } // ""
    public string CreditPoint { get; set; } // 3
    public string RequireOption { get; set; } // Required = R, Not required = E
    public string AllYear { get; set; } // Full Year = F, Half Year = H
    public int ChooseStudent { get; set; } // 42
    public string Restrict1 { get; set; } // 9999
    public string Restrict2 { get; set; } // 55
    public int ThreeStudent { get; set; } // 0
    public int AllStudent { get; set; } // 42
    public string NTURestrict { get; set; } // 0
    public string NTNURestrict { get; set; } // 0
    public string CourseTimes { get; set; } // 3
    public string PracticalTimes { get; set; } // 0
    public string ClassRoomNo { get; set; } // IB-608
    public string? ThreeNode { get; set; } // null
    public string Node { get; set; } // W2,W3,W4
    public string Contents { get; set; } // ""
    public int NTU_People { get; set; } // 0
    public int NTNU_People { get; set; } // 0
    public int AbroadPeople { get; set; } // 0
}