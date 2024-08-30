using PlatformService.Models;
namespace PlatformService.Data.Seeding

{
	public static class PrepDb
	{
		public static void PrepPlatforms(this IApplicationBuilder app)
		{
			using (var servicescope = app.ApplicationServices.CreateScope())
			{
				SeedData(servicescope.ServiceProvider.GetService<PlatformDbContext>());
			}
		}

		public static void SeedData(PlatformDbContext context)
		{
			if (!context.Platform.Any())
			{
				Console.WriteLine("---- Seeding Platform Data");
				context.Platform.AddRange(
					new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
					new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
					new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
					);
				context.SaveChanges();
			}
			else
				Console.WriteLine("-----No Platform data to be seed");

		}

	}
}
