using SchoolApi.DTOs.Enrollments;
using SchoolApi.Services;
using SchoolApi.Tests.Fakes;
using Xunit;

namespace SchoolApi.Tests;

public class EnrollmentRulesTests
{
    private readonly FakeEnrollmentRepository _fakeRepo;
    private readonly EnrollmentsService _service;

    public EnrollmentRulesTests()
    {
        _fakeRepo = new FakeEnrollmentRepository();
        _service = new EnrollmentsService(_fakeRepo);
    }

    [Fact]
    public async Task AddEnrollment_WithValidStudentAndCourse_SuccessfullyEnrolls()
    {
        _fakeRepo.AddCourse(id: 1, capacity: 10);
        _fakeRepo.AddStudent(id: 1);

        var dto = new CreateEnrollmentsDTO { StudentId = 1, CourseId = 1 };

        int enrollmentId = await _service.AddEnrollmentAsync(dto);

        Assert.True(enrollmentId > 0);
        Assert.Single(_fakeRepo.Enrollments);
    }

    [Fact]
    public async Task AddEnrollment_WhenCourseCapacityIsExceeded_ThrowsInvalidOperationException()
    {
        _fakeRepo.AddCourse(id: 1, capacity: 1);
        _fakeRepo.AddStudent(id: 1);
        _fakeRepo.AddStudent(id: 2);

        await _service.AddEnrollmentAsync(new CreateEnrollmentsDTO { StudentId = 1, CourseId = 1 });

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.AddEnrollmentAsync(new CreateEnrollmentsDTO { StudentId = 2, CourseId = 1 }));

        Assert.Equal("Capacity Exceeded", ex.Message);
    }

    [Fact]
    public async Task AddEnrollment_WhenStudentAlreadyEnrolled_ThrowsInvalidOperationException()
    {
        _fakeRepo.AddCourse(id: 1, capacity: 10);
        _fakeRepo.AddStudent(id: 1);

        var dto = new CreateEnrollmentsDTO { StudentId = 1, CourseId = 1 };
        await _service.AddEnrollmentAsync(dto);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddEnrollmentAsync(dto));
        Assert.Equal("Student is already enrolled in the course.", ex.Message);
    }

    [Fact]
    public async Task AddEnrollment_WhenCourseDoesNotExist_ThrowsInvalidOperationException()
    {
        _fakeRepo.AddStudent(id: 1);
        var dto = new CreateEnrollmentsDTO { StudentId = 1, CourseId = 999 };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddEnrollmentAsync(dto));
        Assert.Equal("Course Does not exist.", ex.Message);
    }

    [Fact]
    public async Task AddEnrollment_WhenStudentDoesNotExist_ThrowsInvalidOperationException()
    {
        _fakeRepo.AddCourse(id: 1, capacity: 10);
        var dto = new CreateEnrollmentsDTO { StudentId = 999, CourseId = 1 };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddEnrollmentAsync(dto));
        Assert.Equal("Student id does not exist", ex.Message);
    }
}
