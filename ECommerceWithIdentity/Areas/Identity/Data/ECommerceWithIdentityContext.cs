using ECommerceWithIdentity.Areas.Identity.Data.DataModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWithIdentity.Data;

public class ECommerceWithIdentityContext : IdentityDbContext<IdentityUser>
{
    public ECommerceWithIdentityContext(DbContextOptions<ECommerceWithIdentityContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Permissions>().HasData(
            new { Id = 1, Name = "Create_user"},
            new { Id = 2, Name = "Update_user"}
        );
        
    }
    public DbSet<Permissions> Permissions { get; set; }
}
