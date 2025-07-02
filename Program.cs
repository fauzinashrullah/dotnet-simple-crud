using Microsoft.EntityFrameworkCore;
using SimpleApi;

var builder = WebApplication.CreateBuilder(args);

// Tambahkan DB Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
