using ChatApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChatMedia> ChatMedias { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
    }
}