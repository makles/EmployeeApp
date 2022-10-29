using EmployeeApp.Data;
using EmployeeApp.IRepository;
using EmployeeApp.Repository;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserDBContext>(x =>
{
    x.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepostitory>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "test",
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("test");
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
