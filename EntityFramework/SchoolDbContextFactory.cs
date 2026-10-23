using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DatabaseCourseExam.EntityFramework;

public static class SchoolDbContextFactory
{
    private static readonly IConfiguration Configuration =
        new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

    public static SchoolDbContext Create()
    {
        string connectionString =
            Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Не найдена строка подключения DefaultConnection.");

        var options = new DbContextOptionsBuilder<SchoolDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new SchoolDbContext(options);
    }
}