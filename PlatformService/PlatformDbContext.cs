using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService
{
	public class PlatformDbContext :DbContext
	{
        public PlatformDbContext(DbContextOptions<PlatformDbContext> opt) :base(opt)
        {
                
        }
        public DbSet<Platform> Platform { get; set; }
    }
}
