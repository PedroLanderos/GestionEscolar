using ClassroomApi.Infrastructure.Data;
using ClassroomApi.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddInfrastructureService(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ClassroomDbContext>();
    context.Database.Migrate();
}

app.UseInfrastructurePolicy();


app.UseSwagger();   
app.UseSwaggerUI();

//app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();