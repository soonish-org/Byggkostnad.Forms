using System;
using Microsoft.EntityFrameworkCore;
namespace ByggKostnad.Forms.Data
{
    public class FormsDbContext: DbContext
    {
		public FormsDbContext(DbContextOptions<FormsDbContext> options): base(options){ }

		public DbSet<Response> Responses { get; set; }
    }
}
