using LearnDapper;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerService();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();
app.UsePathBase("/api/v1");

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection(); 
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.MapControllers();

app.Run();

