using backend.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}

    public DbSet<Ingredient> Ingredients { get; set; } = default!;

    public DbSet<Pancake> Pancake { get; set; } = default!;

    public DbSet<Order> Order { get; set; } = default!;

}