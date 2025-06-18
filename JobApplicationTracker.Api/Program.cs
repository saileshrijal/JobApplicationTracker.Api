using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Interface;
using JobApplicationTracker.Api.Data.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//Register services
builder.Services.AddScoped<IJobTypeService, JobTypeService>();

JobApplicationTrackerConfig.ConnectionString = builder.Configuration.GetValue<string>("ConnectionString");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();