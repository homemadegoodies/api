using Data.Contexts;
using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using API.Services;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var envDbHost = Env.GetString("DB_HOST");
var envDbPort = Env.GetInt("DB_PORT");
var envDbDatabase = Env.GetString("DB_DATABASE");
var envDbUsername = Env.GetString("DB_USERNAME");
var envDbPassword = Env.GetString("DB_PASSWORD");

// var localConnectionString = builder.Configuration.GetConnectionString("LocalDbString");

var liveConnectionString = builder.Configuration.GetConnectionString("LiveDbString")
    .Replace("{DB_HOST}", envDbHost)
    .Replace("{DB_PORT}", envDbPort.ToString())
    .Replace("{DB_DATABASE}", envDbDatabase)
    .Replace("{DB_USERNAME}", envDbUsername)
    .Replace("{DB_PASSWORD}", envDbPassword);

// Configure database connections
builder.Services.AddDbContext<GoodiesDataContext>(
    options => options.UseNpgsql(liveConnectionString));

// For InvalidCastException
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add StripeService to the container
builder.Services.AddScoped<StripeService>();

// Add GoogleService to the container
builder.Services.AddScoped<GoogleService>();

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(options =>
    options.WithOrigins("*")
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseAuthorization();
app.MapControllers();
app.Run();
