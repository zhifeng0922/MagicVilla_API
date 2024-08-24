//using MagicVilla_VillaAPI.logging;
//using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc;
//using MagicVilla_VillaAPI.Logging;
//using Microsoft.EntityFrameworkCore;
//using MagicVilla_VillaAPI.Data;
//using Microsoft.EntityFrameworkCore
//using Microsoft.EntityFrameworkCore.SqlServer
//using Microsoft.EntityFrameworkCore.Tools
//using Microsoft.EntityFrameworkCore.Design
using MagicVilla_VillaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//这里use seriLog to log that to files， 如果不考虑继续用就uninstall Serilog；Serilog.AspNetCore；Serilog.Sinks.File
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .WriteTo.File("log/villaLogs.txt",rollingInterval:RollingInterval.Day)
//    .CreateLogger();

//builder.Host.UseSerilog();
//这里use seriLog to log that to files， 因为要用default implementation

//////builder.Services.AddDbContext<ApplicationDbContext>(options =>
//////options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddControllers().AddNewtonsoftJson;
builder.Services.AddControllers(option =>
{
    //option.ReturnHttpNotAcceptable=true;
}).AddNewtonsoftJson() . AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<ILogging, LoggingV2>();

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

app.Run();
