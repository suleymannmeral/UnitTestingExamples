using Microsoft.EntityFrameworkCore;
using UnderstandingDependencies.Api.Models;

namespace UnderstandingDependencies.Api.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }


    }
}
