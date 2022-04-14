using System.Reflection;
using Core.Services;
using Data;
using Data.Migrations;
using Data.Repositories;
using Data.Services;
using Data.UnitOfWork;
using FluentMigrator.Runner;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Serilog
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddSingleton<Database>();

builder.Services.AddLogging(c => c.AddFluentMigratorConsole())
    .AddFluentMigratorCore()
    .ConfigureRunner(c => c.AddPostgres()
        .WithGlobalConnectionString(builder.Configuration.GetConnectionString("InfotecsMonitoring"))
        .ScanIn(Assembly.GetAssembly(typeof(MigrationManager))).For.Migrations());

builder.Services.AddCors();

builder.Services.AddControllers();

builder.Services.AddScoped<DbSession>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IConnectionMonitoringRepository, ConnectionMonitoringRepository>();
builder.Services.AddTransient<IConnectionInfoService, ConnectionInfoService>();
builder.Services.AddTransient<IConnectionEventService, ConnectionEventService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase<Program>().Run();
