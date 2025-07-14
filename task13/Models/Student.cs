using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace practice2025.Task13.Models;

public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime BirthDate { get; set; }

    public List<Subject> Grades { get; set; }
}
