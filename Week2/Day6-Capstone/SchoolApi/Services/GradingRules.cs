namespace SchoolApi.Services;

public static class GradingRules
{
    public static string CalculateGrade(int marks)
    {
        if (marks < 0 || marks > 100)
        {
            throw new ArgumentException("Marks should be between 0-100", nameof(marks));
        }

        if (marks >= 90)
        {
            return "A";
        }
        if (marks >= 80)
        {
            return "B";
        }
        if (marks >= 70)
        {
            return "C";
        }
        if (marks >= 60)
        {
            return "D";
        }
        return "F";
    }
}

