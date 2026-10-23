using DatabaseCourseExam.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseCourseExam.EntityFramework;

public static class EntityFrameworkTasks
{
    public static async Task ShowStudentsAsync()
    {
        await using var context = SchoolDbContextFactory.Create();

        List<Student> students = await context.Students
            .Include(student => student.Group)
            .OrderBy(student => student.Id)
            .ToListAsync();

        foreach (Student student in students)
        {
            Console.WriteLine(
                $"{student.Id}. {student.FirstName} {student.LastName}, " +
                $"возраст: {student.Age}, группа: {student.Group.Name}");
        }
    }

    public static async Task UpdateStudentAgeAsync(
        int studentId,
        int newAge)
    {
        await using var context = SchoolDbContextFactory.Create();

        Student? student = await context.Students
            .FirstOrDefaultAsync(x => x.Id == studentId);

        if (student is null)
        {
            Console.WriteLine("Студент не найден.");
            return;
        }

        student.Age = newAge;

        int affectedRows = await context.SaveChangesAsync();

        Console.WriteLine(
            $"Возраст изменён. Сохранено изменений: {affectedRows}");
    }

    // TODO (задание 4): Выбирать только нужные поля через Select и добавить вычисляемый признак «мест нет».
    public static async Task ShowCoursesAsync()
    {
        await using var context = SchoolDbContextFactory.Create();

        var courses = await context.Courses
            .OrderBy(course => course.Name)
            .Select(course => new
            {
                course.Name,
                course.Price,
                course.AvailablePlaces,
                // Вычисляемый признак: true, если мест 0 или меньше
                NoPlacesLeft = course.AvailablePlaces <= 0
            })
            .ToListAsync();

        foreach (var course in courses)
        {
            // Формируем строку вывода в зависимости от вычисляемого признака
            string placesInfo = course.NoPlacesLeft
                ? "Мест нет"
                : course.AvailablePlaces.ToString();

            Console.WriteLine(
                $"{course.Name} | Цена: {course.Price:N2} | " +
                $"Свободных мест: {placesInfo}");
        }
    }
}