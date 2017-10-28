using System;
using Microsoft.EntityFrameworkCore;
namespace Soonish.Forms.Data
{
    public class FormsDbContext : DbContext
    {
        public FormsDbContext(DbContextOptions<FormsDbContext> options) : base(options) { }

        public DbSet<Response> Responses { get; set; }
    }
}
