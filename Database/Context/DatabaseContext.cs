using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace Database.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(AppConfig.ConnectionStrings?.DefaultConnection).LogTo(Console.WriteLine);
        
        if (AppConfig.ConsoleLogQueries)
        {
            optionsBuilder.LogTo(Console.WriteLine);
        }
    }
}