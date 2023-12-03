
using CRUDApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}