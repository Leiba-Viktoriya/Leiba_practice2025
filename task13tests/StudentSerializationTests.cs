using System;
using System.IO;
using System.Collections.Generic;
using Xunit;
using practice2025.Task13;
using practice2025.Task13.Models;

namespace task13tests
{
    public class StudentSerializationTests
    {
        private readonly Student sample = new()
        {
            FirstName = "Ivan",
            LastName = "Petrov",
            BirthDate = new DateTime(2000, 5, 15),
            Grades = new List<Subject>
            {
                new Subject { Name = "Math", Grade = 5 },
                new Subject { Name = "Physics", Grade = 4 }
            }
        };

        [Fact]
        public void SerializeStudent_IncludesPropertiesAndDateFormat()
        {
            var json = JsonHelper.SerializeStudent(sample);
            Assert.Contains("\"FirstName\": \"Ivan\"", json);
            Assert.Contains("\"BirthDate\": \"2000-05-15\"", json);
        }

        [Fact]
        public void DeserializeStudent_ValidJson_ReturnsStudent()
        {
            var json = JsonHelper.SerializeStudent(sample);
            var student = JsonHelper.DeserializeStudent(json);
            Assert.Equal(sample.FirstName, student.FirstName);
            Assert.Equal(sample.LastName, student.LastName);
            Assert.Equal(sample.BirthDate, student.BirthDate);
        }

        [Fact]
        public void DeserializeStudent_InvalidData_Throws()
        {
            var invalid = "{\"FirstName\":\"\",\"LastName\":\"\",\"BirthDate\":\"2000-05-15\",\"Grades\":null}";
            Assert.Throws<InvalidDataException>(() => JsonHelper.DeserializeStudent(invalid));
        }

        [Fact]
        public void SaveAndLoad_FileOperationsWork()
        {
            var path = Path.GetTempFileName();
            try
            {
                var json = JsonHelper.SerializeStudent(sample);
                JsonHelper.SaveToFile(path, json);
                var loaded = JsonHelper.LoadFromFile(path);
                Assert.Equal(json, loaded);
            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}
