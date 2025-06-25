using System.Collections.Generic;
using System.Linq;

namespace task02
{
    public class Learner
    {
        public string Name { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public List<int> Grades { get; set; } = new();
    }

    public class LearnerService
    {
        private readonly IReadOnlyCollection<Learner> _data;

        public LearnerService(IReadOnlyCollection<Learner> data) => _data = data;

        public IEnumerable<Learner> GetByFaculty(string faculty) =>
            from student in _data where student.Faculty == faculty select student;

        public IEnumerable<Learner> GetWithMinAverage(double threshold) =>
            _data.Where(s => s.Grades.Average() >= threshold);

        public IEnumerable<Learner> SortByName() =>
            _data.OrderBy(s => s.Name);

        public IEnumerable<IGrouping<string, Learner>> GroupByFaculty() =>
            _data.GroupBy(s => s.Faculty);

        public string GetTopFaculty()
        {
            var stats = from groupData in _data
                        group groupData by groupData.Faculty into grp
                        select new
                        {
                            Faculty = grp.Key,
                            Average = grp.SelectMany(x => x.Grades).DefaultIfEmpty().Average()
                        };

            return stats.OrderByDescending(g => g.Average).First().Faculty;
        }
    }
}
