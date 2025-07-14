using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using practice2025.Task13.Models;

namespace practice2025.Task13;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    public static string SerializeStudent(Student student)
    {
        return JsonSerializer.Serialize(student, Options);
    }

    public static Student DeserializeStudent(string json)
    {
        var student = JsonSerializer.Deserialize<Student>(json, Options)
                      ?? throw new InvalidDataException("JSON не соответствует модели Student");

        if (string.IsNullOrWhiteSpace(student.FirstName)
            || string.IsNullOrWhiteSpace(student.LastName)
            || student.Grades == null)
        {
            throw new InvalidDataException("Некорректные данные студента");
        }

        return student;
    }

    public static void SaveToFile(string path, string json)
    {
        File.WriteAllText(path, json);
    }

    public static string LoadFromFile(string path)
    {
        return File.ReadAllText(path);
    }
}

public class DateTimeConverter : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-dd";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (DateTime.TryParseExact(reader.GetString()!, Format, null, System.Globalization.DateTimeStyles.None, out var dt))
            return dt;
        throw new JsonException($"Неверный формат даты, ожидалось {Format}");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}
