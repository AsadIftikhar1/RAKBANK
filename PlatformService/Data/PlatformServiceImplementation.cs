using PlatformService.Models;

namespace PlatformService.Data
{
	public class PlatformServiceImplementation : IPlatformRepo
	{
		private readonly PlatformDbContext _context;

		public PlatformServiceImplementation(PlatformDbContext platformDbContext)
        {
			_context = platformDbContext;
		}
        public void CreatePlatform(Platform platform)
		{
			if (platform == null) throw new ArgumentNullException(nameof(platform));
			_context.Platform.Add(platform);
		}

		public IEnumerable<Platform> GetAllPlatforms()
		{
			return _context.Platform.ToList();
		}

		public Platform GetPlatformById(int Id)
		{
			return _context.Platform.FirstOrDefault(p => p.ID == Id);
		}

		public bool SaveChanges()
		{
			return (_context.SaveChanges()>=0);
		}
	}
}
