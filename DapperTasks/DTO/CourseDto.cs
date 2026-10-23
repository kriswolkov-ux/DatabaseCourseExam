namespace DatabaseCourseExam.DapperTasks.DTO;

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int AvailablePlaces { get; set; }
}