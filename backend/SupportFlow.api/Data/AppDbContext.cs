using Microsoft.EntityFrameworkCore;

namespace SupportFlow.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
    {
        
    }
}