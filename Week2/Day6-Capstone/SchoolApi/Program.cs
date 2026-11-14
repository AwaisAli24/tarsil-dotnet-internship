using Npgsql;
using SchoolApi.Data;
using SchoolApi.Exceptions;
using SchoolApi.Repositories;
using SchoolApi.Repositories.Interfaces;
using SchoolApi.Repository;
using SchoolApi.Repository.Interfaces;
using SchoolApi.Services;
using SchoolApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Configuration.AddUserSecrets<Program>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString == null)
{
    throw new ArgumentException("Connection string is not set");
}

builder.Services.AddSingleton(NpgsqlDataSource.Create(connectionString));

builder.Services.AddScoped<IClassRoomsRepository, ClassRoomsRepository>();
builder.Services.AddScoped<IClassRoomsService, ClassRoomsService>();

builder.Services.AddScoped<IStudentsRepository, StudentsRepository>();
builder.Services.AddScoped<IStudentsService, StudentsService>();

builder.Services.AddScoped<ITeachersRepository, TeachersRepository>();
builder.Services.AddScoped<ITeacherService, TeacherService>();

builder.Services.AddScoped<ICoursesRepository, CoursesRepository>();
builder.Services.AddScoped<ICoursesService, CoursesService>();

builder.Services.AddScoped<IEnrollmentRepository, EnrollmentsRepository>();
builder.Services.AddScoped<IEnrollmentsService, EnrollmentsService>();

builder.Services.AddScoped<IReportsRepository, ReportsRepository>();
builder.Services.AddScoped<IReportsService, ReportsService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddSingleton<DataBaseMigrator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var migrator = app.Services.GetRequiredService<DataBaseMigrator>();
await migrator.RunAsync();

app.UseExceptionHandler(_ => { });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
