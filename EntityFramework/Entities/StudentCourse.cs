namespace DatabaseCourseExam.EntityFramework.Entities;

public class StudentCourse
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrolledAt { get; set; }
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
}