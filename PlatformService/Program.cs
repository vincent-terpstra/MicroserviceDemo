using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(
        opt => opt.UseInMemoryDatabase("InMem")
    );
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Host.UseSerilog(
    (context, config)=> config.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.PrepPopulation(Log.Logger);
try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "A fatal error occured while running the app!");
}
finally
{
    Log.CloseAndFlush();
}