using SchoolApi.Services;
using SchoolApi.Tests.Fakes;
using Xunit;

namespace SchoolApi.Tests;

public class GradingRulesTests
{
    private readonly FakeEnrollmentRepository _fakeRepo;
    private readonly EnrollmentsService _service;

    public GradingRulesTests()
    {
        _fakeRepo = new FakeEnrollmentRepository();
        _service = new EnrollmentsService(_fakeRepo);
    }

    [Fact]
    public void CalculateGrade_WhenMarksAre90OrAbove_ReturnsGradeA()
    {
        Assert.Equal("A", GradingRules.CalculateGrade(90));
        Assert.Equal("A", GradingRules.CalculateGrade(100));
    }

    [Fact]
    public void CalculateGrade_WhenMarksAreBetween80And89_ReturnsGradeB()
    {
        Assert.Equal("B", GradingRules.CalculateGrade(80));
        Assert.Equal("B", GradingRules.CalculateGrade(89));
    }

    [Fact]
    public void CalculateGrade_WhenMarksAreBetween70And79_ReturnsGradeC()
    {
        Assert.Equal("C", GradingRules.CalculateGrade(70));
        Assert.Equal("C", GradingRules.CalculateGrade(79));
    }

    [Fact]
    public void CalculateGrade_WhenMarksAreBetween60And69_ReturnsGradeD()
    {
        Assert.Equal("D", GradingRules.CalculateGrade(60));
        Assert.Equal("D", GradingRules.CalculateGrade(69));
    }

    [Fact]
    public void CalculateGrade_WhenMarksAreBelow60_ReturnsGradeF()
    {
        Assert.Equal("F", GradingRules.CalculateGrade(0));
        Assert.Equal("F", GradingRules.CalculateGrade(59));
    }

    [Fact]
    public async Task UpdateMarks_WhenMarksOutOfRange_ThrowsArgumentException()
    {
        var exAbove = await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateMarksAsync(1, 101));
        var exBelow = await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateMarksAsync(1, -1));

        Assert.Equal("Marks should be between 0-100", exAbove.Message);
        Assert.Equal("Marks should be between 0-100", exBelow.Message);
    }
}
