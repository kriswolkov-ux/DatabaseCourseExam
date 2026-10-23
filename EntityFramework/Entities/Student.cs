namespace DatabaseCourseExam.EntityFramework.Entities;

public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public int GroupId { get; set; }
    public StudentGroup Group { get; set; } = null!;
    public List<StudentCourse> StudentCourses { get; set; } = [];
}