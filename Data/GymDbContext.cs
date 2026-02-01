using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
        }
        public DbSet<Member> Members { get; set; }
    }
}
