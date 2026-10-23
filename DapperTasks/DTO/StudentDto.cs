namespace DatabaseCourseExam.DapperTasks.DTO;

public class StudentDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string GroupName { get; set; } = string.Empty;
}