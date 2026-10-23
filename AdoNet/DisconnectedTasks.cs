using System.Data;
using DatabaseCourseExam.Data;
using Npgsql;

namespace DatabaseCourseExam.AdoNet;

public static class DisconnectedTasks
{
    public static async Task<DataSet> LoadStudentsAsync()
    {
        var dataSet = new DataSet();

        await using var connection = DatabaseConnection.Create();
        await connection.OpenAsync();

        const string studentsSql = """
            SELECT id, first_name, last_name, age, group_id
            FROM students
            ORDER BY id;
            """;

        await using var studentsCmd = new NpgsqlCommand(studentsSql, connection);
        using var studentsAdapter = new NpgsqlDataAdapter(studentsCmd);
        studentsAdapter.Fill(dataSet, "Students");
        dataSet.Tables["Students"]!.PrimaryKey = [dataSet.Tables["Students"]!.Columns["id"]!];

        const string enrollmentsSql = """
            SELECT student_id, course_id
            FROM student_courses;
            """;

        await using var enrollmentsCmd = new NpgsqlCommand(enrollmentsSql, connection);
        using var enrollmentsAdapter = new NpgsqlDataAdapter(enrollmentsCmd);
        enrollmentsAdapter.Fill(dataSet, "StudentCourses");

        dataSet.Relations.Add(
            "StudentEnrollments",
            dataSet.Tables["Students"]!.Columns["id"],
            dataSet.Tables["StudentCourses"]!.Columns["student_id"]
        );

        return dataSet;
    }

    public static void PrintTable(DataSet dataSet)
    {
        var studentsTable = dataSet.Tables["Students"]!;
        var relation = dataSet.Relations["StudentEnrollments"]!;

        foreach (DataRow row in studentsTable.Rows)
        {
            var childRows = row.GetChildRows(relation);

            Console.WriteLine(
                $"{row["id"]}: {row["first_name"]} {row["last_name"]}, " +
                $"возраст {row["age"]}, записей на курсы: {childRows.Length}");
        }
    }

    public static void ChangeAgeInMemory(
        DataTable table,
        int studentId,
        int newAge)
    {
        DataRow? row = table.Rows.Find(studentId);

        if (row is null)
        {
            Console.WriteLine("Студент не найден в DataTable.");
            return;
        }

        row["age"] = newAge;

        Console.WriteLine(
            $"Возраст изменён в памяти. Состояние строки: {row.RowState}");
    }

    public static int SaveChanges(DataTable table)
    {
        using var connection = DatabaseConnection.Create();
        connection.Open();

        const string sql = """
        UPDATE students
        SET age = @age
        WHERE id = @id;
        """;

        using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add("@age", NpgsqlTypes.NpgsqlDbType.Integer, 4, "age");
        command.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Integer, 4, "id");

        using var adapter = new NpgsqlDataAdapter();
        adapter.UpdateCommand = command;

        int affectedRows = adapter.Update(table);
        Console.WriteLine($"Сохранено изменений: {affectedRows}");

        var modifiedRows = table.Select(null, null, DataViewRowState.ModifiedCurrent);
        if (modifiedRows.Length > 0)
        {
            Console.WriteLine($"Внимание: осталось {modifiedRows.Length} строк в состоянии Modified.");
        }
        else
        {
            Console.WriteLine("Все измененные строки успешно сохранены (состояние Modified отсутствует).");
        }

        return affectedRows;
    }
}