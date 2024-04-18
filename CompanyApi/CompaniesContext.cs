using CompanyApi.Companies;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi;

public class CompanyDbContext(DbContextOptions<CompanyDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies => Set<Company>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Company>();

        base.OnModelCreating(builder);
    }
}
