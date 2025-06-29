using Xunit;
using Moq;
using task02;
using System.Collections.Generic;
using System.Linq;

namespace task02tests;

public class StudentServiceTests
{
    private readonly List<Student> _students;
    private readonly Mock<IStudentRepository> _repositoryMock;
    private readonly StudentService _service;

    public StudentServiceTests()
    {
        _students = new List<Student>
        {
            new() { Name = "Иван", Faculty = "ФИТ", Grades = new() { 5, 4, 5 } },
            new() { Name = "Анна", Faculty = "ФИТ", Grades = new() { 3, 4, 3 } },
            new() { Name = "Петр", Faculty = "Экономика", Grades = new() { 5, 5, 5 } }
        };

        _repositoryMock = new Mock<IStudentRepository>();
        _repositoryMock.Setup(r => r.GetAllStudents()).Returns(_students);

        _service = new StudentService(_repositoryMock.Object);
    }

    [Fact]
    public void GetStudentsByFaculty_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsByFaculty("ФИТ").ToList();
        Assert.Equal(2, result.Count);
        Assert.All(result, s => Assert.Equal("ФИТ", s.Faculty));
        _repositoryMock.Verify(r => r.GetAllStudents(), Times.Once);
    }

    [Fact]
    public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
    {
        var result = _service.GetFacultyWithHighestAverageGrade();
        Assert.Equal("Экономика", result);
    }

    [Fact]
    public void GetStudentsWithAverageGradeAbove_ReturnsOnlyHighPerformers()
    {
        var result = _service.GetStudentsWithAverageGradeAbove(4.0).ToList();
        Assert.Single(result);
        Assert.Equal("Петр", result[0].Name);
    }

    [Fact]
    public void GetStudentsSortedByName_ReturnsAlphabeticallySorted()
    {
        var result = _service.GetStudentsSortedByName().Select(s => s.Name).ToList();
        Assert.Equal(new List<string> { "Анна", "Иван", "Петр" }, result);
    }

    [Fact]
    public void GroupStudentsByFaculty_ReturnsCorrectGroups()
    {
        var result = _service.GroupStudentsByFaculty();
        Assert.Equal(2, result.Count);
        Assert.Equal(2, result["ФИТ"].Count);
        Assert.Equal(1, result["Экономика"].Count);
    }
}
