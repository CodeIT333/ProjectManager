using API.Configurations;
using Application.Configurations;
using Infrastructure.Configurations;
using Persistence.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSwaggerDocumentation();

builder.Services.AddAPIServices();
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}
app.UseMiddleware<ConfigureException>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
