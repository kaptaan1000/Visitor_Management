using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts) { }
    public DbSet<Visitor> Visitors { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);
        // If legacy char fields exist, you can configure fixed length etc here:
        mb.Entity<Visitor>().Property(v => v.Comp).HasColumnType("char(4)");
    }
}
