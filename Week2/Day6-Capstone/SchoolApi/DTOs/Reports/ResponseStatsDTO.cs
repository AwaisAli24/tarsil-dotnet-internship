namespace SchoolApi.DTOs.Reports;

public class ResponseStatsDTO
{
    public int CourseId { get; set; }
    public double AvgMarks { get; set; }
    public int HighestMarks { get; set; }
    public int LowestMarks { get; set; }
    public List<GradeCount> GradeCounts { get; set; } = new List<GradeCount>();
}
public class GradeCount
{
    public string Grade { get; set; } = string.Empty;
    public int Count { get; set; }
}