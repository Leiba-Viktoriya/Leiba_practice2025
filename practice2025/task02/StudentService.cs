namespace task02;

public class StudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Student> GetStudentsByFaculty(string faculty)
    {
        return _repository.GetAllStudents()
            .Where(s => s.Faculty == faculty);
    }

    public IEnumerable<Student> GetStudentsWithAverageGradeAbove(double threshold)
    {
        return _repository.GetAllStudents()
            .Where(s => s.Grades.Average() > threshold);
    }

    public IEnumerable<Student> GetStudentsSortedByName()
    {
        return _repository.GetAllStudents()
            .OrderBy(s => s.Name);
    }

    public Dictionary<string, List<Student>> GroupStudentsByFaculty()
    {
        return _repository.GetAllStudents()
            .GroupBy(s => s.Faculty)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    public string GetFacultyWithHighestAverageGrade()
    {
        return _repository.GetAllStudents()
            .GroupBy(s => s.Faculty)
            .OrderByDescending(g => g.Average(s => s.Grades.Average()))
            .Select(g => g.Key)
            .FirstOrDefault() ?? "";
    }
}
