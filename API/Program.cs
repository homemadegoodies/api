using Data.Contexts;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var envDbHost = Env.GetString("DB_HOST");
var envDbPort = Env.GetInt("DB_PORT");
var envDbDatabase = Env.GetString("DB_DATABASE");
var envDbUsername = Env.GetString("DB_USERNAME");
var envDbPassword = Env.GetString("DB_PASSWORD");

var connectionString = builder.Configuration.GetConnectionString("LocalDbString")
    .Replace("{DB_HOST}", envDbHost)
    .Replace("{DB_PORT}", envDbPort.ToString())
    .Replace("{DB_DATABASE}", envDbDatabase)
    .Replace("{DB_USERNAME}", envDbUsername)
    .Replace("{DB_PASSWORD}", envDbPassword);

// Configure database connections
builder.Services.AddDbContext<GoodiesDataContext>(
    options => options.UseNpgsql(connectionString));

// For InvalidCastException
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseCors(options =>
    options.WithOrigins("*")
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseAuthorization();
app.MapControllers();
app.Run();
