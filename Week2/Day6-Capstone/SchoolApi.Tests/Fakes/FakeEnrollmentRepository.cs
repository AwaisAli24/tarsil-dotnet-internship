using SchoolApi.DTOs.Enrollments;
using SchoolApi.Repository.Interfaces;
using SchoolApi.Services;

namespace SchoolApi.Tests.Fakes;

public class FakeCourse
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Capacity { get; set; }
}

public class FakeStudent
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class FakeEnrollmentItem
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateOnly EnrolledOn { get; set; }
    public int? Marks { get; set; }
    public string? Grade { get; set; }
}

public class FakeEnrollmentRepository : IEnrollmentRepository
{
    public List<FakeCourse> Courses { get; } = new();
    public List<FakeStudent> Students { get; } = new();
    public List<FakeEnrollmentItem> Enrollments { get; } = new();

    private int _nextEnrollmentId = 1;

    public void AddCourse(int id, int capacity, string code = "CS101", string title = "Computer Science")
    {
        Courses.Add(new FakeCourse
        {
            Id = id,
            Capacity = capacity,
            Code = code,
            Title = title
        });
    }

    public void AddStudent(int id, string name = "John Doe")
    {
        Students.Add(new FakeStudent
        {
            Id = id,
            Name = name
        });
    }

    public Task<int> AddEnrollmentAsync(CreateEnrollmentsDTO enrollment)
    {
        var course = Courses.FirstOrDefault(c => c.Id == enrollment.CourseId);
        if (course == null)
        {
            throw new InvalidOperationException("Course Does not exist.");
        }

        var student = Students.FirstOrDefault(s => s.Id == enrollment.StudentId);
        if (student == null)
        {
            throw new InvalidOperationException("Student id does not exist");
        }

        int currentlyEnrolled = Enrollments.Count(e => e.CourseId == enrollment.CourseId);
        if (currentlyEnrolled + 1 > course.Capacity)
        {
            throw new InvalidOperationException("Capacity Exceeded");
        }

        bool exists = Enrollments.Any(e => e.CourseId == enrollment.CourseId && e.StudentId == enrollment.StudentId);
        if (exists)
        {
            throw new InvalidOperationException("Student is already enrolled in the course.");
        }

        var item = new FakeEnrollmentItem
        {
            Id = _nextEnrollmentId++,
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId,
            EnrolledOn = enrollment.EnrolledOn == default ? DateOnly.FromDateTime(DateTime.Now) : enrollment.EnrolledOn
        };

        Enrollments.Add(item);
        return Task.FromResult(item.Id);
    }

    public Task<bool> UpdateMarksAsync(int id, int marks)
    {
        var enrollment = Enrollments.FirstOrDefault(e => e.Id == id);
        if (enrollment == null)
        {
            return Task.FromResult(false);
        }

        enrollment.Marks = marks;
        enrollment.Grade = GradingRules.CalculateGrade(marks);
        return Task.FromResult(true);
    }
}

