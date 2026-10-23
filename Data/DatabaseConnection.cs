
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace DatabaseCourseExam.Data;

public static class DatabaseConnection
{
    private static readonly IConfiguration Configuration =
        new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

    private static readonly string ConnectionString =
        Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "Не найдена строка подключения DefaultConnection.");

    public static NpgsqlConnection Create()
    {
        return new NpgsqlConnection(ConnectionString);
    }
}