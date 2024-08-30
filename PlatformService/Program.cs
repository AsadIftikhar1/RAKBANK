using Microsoft.EntityFrameworkCore;
using PlatformService;
using PlatformService.Data;
using PlatformService.Data.Seeding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PlatformDbContext>(
	x=>x.UseInMemoryDatabase("InMem"));

builder.Services.AddScoped<IPlatformRepo, PlatformServiceImplementation>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.PrepPlatforms();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
