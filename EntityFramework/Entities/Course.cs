namespace DatabaseCourseExam.EntityFramework.Entities;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int AvailablePlaces { get; set; }
    public List<StudentCourse> StudentCourses { get; set; } = [];
}