using Application.Common.Interfaces;
using Application.DependencyInjection;
using Application.Mappings;
using Application.Services;
using DAL.SqlServer.DependencyInjection;
using DAL.SqlServer.UnitOfWork;
using Microsoft.AspNetCore.Diagnostics;
using Repository.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddApplicationServices();
// builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddApplicationServices();


//bizde ayri ayri yazildigina gore bu qaydada vereceyik
builder.Services.AddAutoMapper(typeof(PostProfile).Assembly);


//db elave eledim 
#region database


var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSqlServerPersistence(conn);

//var conn = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(conn));


#endregion

builder.Services.AddScoped<IUnitOfWork, SqlUnitOfWork>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ProFix API V1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

//yeni elave
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers(); 

app.Run();
