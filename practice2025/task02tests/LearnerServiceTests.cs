using System.Collections.Generic;
using System.Linq;
using task02;
using Xunit;

namespace task02tests
{
    public class LearnerServiceTests
    {
        private List<Learner> SampleData() => new()
        {
            new() { Name = "Алексей", Faculty = "Менеджмент", Grades = new() { 5, 4, 5 } },
            new() { Name = "Вероника", Faculty = "Менеджмент", Grades = new() { 3, 3, 4 } },
            new() { Name = "Светлана", Faculty = "ИТ", Grades = new() { 5, 5, 5 } },
            new() { Name = "Никита", Faculty = "ИТ", Grades = new() { 4, 5, 5 } }
        };

        [Fact]
        public void GetByFaculty_ReturnsMatchingStudents()
        {
            var service = new LearnerService(SampleData());
            var result = service.GetByFaculty("Менеджмент").ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, x => Assert.Equal("Менеджмент", x.Faculty));
        }

        [Fact]
        public void GetWithMinAverage_ReturnsOnlyQualified()
        {
            var service = new LearnerService(SampleData());
            var result = service.GetWithMinAverage(4.7).Select(l => l.Name).ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains("Светлана", result);
            Assert.Contains("Никита", result);
        }

        [Fact]
        public void SortByName_ReturnsAlphabeticalOrder()
        {
            var service = new LearnerService(SampleData());
            var names = service.SortByName().Select(l => l.Name).ToList();

            Assert.Equal(new[] { "Алексей", "Вероника", "Никита", "Светлана" }, names);
        }

        [Fact]
        public void GroupByFaculty_ReturnsCorrectGroups()
        {
            var service = new LearnerService(SampleData());
            var grouped = service.GroupByFaculty().ToDictionary(g => g.Key);

            Assert.Equal(2, grouped.Count);
            Assert.Equal(2, grouped["ИТ"].Count());
            Assert.Equal(2, grouped["Менеджмент"].Count());
        }

        [Fact]
        public void GetTopFaculty_ReturnsFacultyWithHighestAverage()
        {
            var service = new LearnerService(SampleData());
            var top = service.GetTopFaculty();

            Assert.Equal("ИТ", top);
        }
    }
}
