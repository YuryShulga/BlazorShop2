using Microsoft.EntityFrameworkCore;

namespace BlazorShop.Models
{
    public class AppDbContext : DbContext
    {
        //Список таблиц:
        public DbSet<Product> Products { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public AppDbContext(DbContextOptions<AppDbContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {
        }
    }

}
