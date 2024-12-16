using Auth.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql("Server=mathapp-tests.postgres.database.azure.com;Database=postgres;Port=5432;User Id=mathapp;Password=projektZespolowy123;Ssl Mode=Require;");

        return new AppDbContext(optionsBuilder.Options);
    }
}
