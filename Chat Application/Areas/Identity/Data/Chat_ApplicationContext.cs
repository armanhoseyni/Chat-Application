using Chat_Application.Areas.Identity.Data;
using Chat_Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DB;

public class Chat_ApplicationContext : IdentityDbContext<ApplicationUser>
{
    public Chat_ApplicationContext(DbContextOptions<Chat_ApplicationContext> options)
        : base(options)
    {
    }
    public DbSet<UserMessage> UserMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
