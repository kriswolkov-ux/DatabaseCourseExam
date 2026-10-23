// TODO (задание 7): Заменить int.Parse на int.TryParse; неверный ввод не должен завершать программу.
// TODO (задание 2): Добавить пункт меню, выводящий список студентов отдельными методами ADO.NET, EF Core и Dapper.

using System.Data;
using DatabaseCourseExam.AdoNet;
using DatabaseCourseExam.EntityFramework;
using DatabaseCourseExam.DapperTasks;

DataSet? studentsData = null;

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== Зачёт по работе с БД ===");
    Console.WriteLine("1. Показать студентов (ADO.NET)");
    Console.WriteLine("2. Изменить возраст студента (ADO.NET)");
    Console.WriteLine("3. Записать студента на курс (ADO.NET)");
    Console.WriteLine("4. Загрузить студентов в DataSet (с курсами)");
    Console.WriteLine("5. Изменить возраст в DataSet");
    Console.WriteLine("6. Показать DataSet");
    Console.WriteLine("7. Сохранить изменения DataSet");
    Console.WriteLine("8. EF Core: показать студентов");
    Console.WriteLine("9. EF Core: изменить возраст студента");
    Console.WriteLine("10. EF Core: показать курсы");
    Console.WriteLine("11. Dapper: показать студентов");
    Console.WriteLine("12. Dapper: изменить возраст");
    Console.WriteLine("13. Dapper: показать курсы");
    Console.WriteLine("14. Dapper: записать студента на курс");
    Console.WriteLine("15. Сравнение: показать студентов (ADO.NET, EF Core, Dapper)");
    Console.WriteLine("0. Выход");
    Console.Write("Выберите действие: ");

    string? choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                await AdoNetTasks.ShowStudentsAsync();
                break;

            case "2":
                Console.Write("ID студента: ");
                if (!int.TryParse(Console.ReadLine(), out int studentId))
                {
                    Console.WriteLine("Некорректный ID.");
                    break;
                }

                Console.Write("Новый возраст: ");
                if (!int.TryParse(Console.ReadLine(), out int age))
                {
                    Console.WriteLine("Некорректный возраст.");
                    break;
                }

                await AdoNetTasks.UpdateStudentAgeAsync(studentId, age);
                break;

            case "3":
                Console.Write("ID студента: ");
                if (!int.TryParse(Console.ReadLine(), out int enrollmentStudentId))
                {
                    Console.WriteLine("Некорректный ID студента.");
                    break;
                }

                Console.Write("ID курса: ");
                if (!int.TryParse(Console.ReadLine(), out int courseId))
                {
                    Console.WriteLine("Некорректный ID курса.");
                    break;
                }

                await AdoNetTasks.EnrollStudentAsync(enrollmentStudentId, courseId);
                break;

            case "4":
                studentsData = await DisconnectedTasks.LoadStudentsAsync();
                Console.WriteLine("Данные загружены в память.");
                DisconnectedTasks.PrintTable(studentsData);
                break;

            case "5":
                if (studentsData is null)
                {
                    Console.WriteLine("Сначала загрузите данные (пункт 4).");
                    break;
                }

                Console.Write("ID студента: ");
                if (!int.TryParse(Console.ReadLine(), out int disconnectedStudentId))
                {
                    Console.WriteLine("Некорректный ID.");
                    break;
                }

                Console.Write("Новый возраст: ");
                if (!int.TryParse(Console.ReadLine(), out int disconnectedAge) || disconnectedAge is < 14 or > 100)
                {
                    Console.WriteLine("Возраст должен быть целым числом от 14 до 100.");
                    break;
                }

                DisconnectedTasks.ChangeAgeInMemory(
                    studentsData.Tables["Students"]!,
                    disconnectedStudentId,
                    disconnectedAge);
                break;

            case "6":
                if (studentsData is null)
                {
                    Console.WriteLine("Сначала загрузите данные (пункт 4).");
                    break;
                }

                DisconnectedTasks.PrintTable(studentsData);
                break;

            case "7":
                if (studentsData is null)
                {
                    Console.WriteLine("Сначала загрузите данные (пункт 4).");
                    break;
                }

                DisconnectedTasks.SaveChanges(studentsData.Tables["Students"]!);
                break;

            case "8":
                await EntityFrameworkTasks.ShowStudentsAsync();
                break;

            case "9":
                {
                    Console.Write("ID студента: ");
                    if (!int.TryParse(Console.ReadLine(), out int stId))
                    {
                        Console.WriteLine("Некорректный ID.");
                        break;
                    }

                    Console.Write("Новый возраст: ");
                    if (!int.TryParse(Console.ReadLine(), out int newAge) || newAge is < 14 or > 100)
                    {
                        Console.WriteLine("Возраст должен быть целым числом от 14 до 100.");
                        break;
                    }

                    await EntityFrameworkTasks.UpdateStudentAgeAsync(stId, newAge);
                    break;
                }

            case "10":
                await EntityFrameworkTasks.ShowCoursesAsync();
                break;

            case "11":
                await DapperStudentTasks.ShowStudentsAsync();
                break;

            case "12":
                {
                    Console.Write("ID студента: ");
                    if (!int.TryParse(Console.ReadLine(), out int dStudentId))
                    {
                        Console.WriteLine("Некорректный ID.");
                        break;
                    }

                    Console.Write("Новый возраст: ");
                    if (!int.TryParse(Console.ReadLine(), out int dNewAge) || dNewAge is < 14 or > 100)
                    {
                        Console.WriteLine("Возраст должен быть целым числом от 14 до 100.");
                        break;
                    }

                    await DapperStudentTasks.UpdateStudentAgeAsync(dStudentId, dNewAge);
                    break;
                }

            case "13":
                await DapperStudentTasks.ShowCoursesAsync();
                break;

            case "14":
                {
                    Console.Write("ID студента: ");
                    if (!int.TryParse(Console.ReadLine(), out int dEnrollStudentId))
                    {
                        Console.WriteLine("Некорректный ID.");
                        break;
                    }

                    Console.Write("ID курса: ");
                    if (!int.TryParse(Console.ReadLine(), out int dCourseId))
                    {
                        Console.WriteLine("Некорректный ID.");
                        break;
                    }

                    await DapperStudentTasks.EnrollStudentAsync(dEnrollStudentId, dCourseId);
                    break;
                }

            case "15":
                Console.WriteLine("\n=== Результаты ADO.NET ===");
                await AdoNetTasks.ShowStudentsAsync();

                Console.WriteLine("\n=== Результаты EF Core ===");
                await EntityFrameworkTasks.ShowStudentsAsync();

                Console.WriteLine("\n=== Результаты Dapper ===");
                await DapperStudentTasks.ShowStudentsAsync();
                break;

            case "0":
                return;

            default:
                Console.WriteLine("Неизвестная команда.");
                break;
        }
    }
    catch (FormatException)
    {
        Console.WriteLine("Введите целое число.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }
}