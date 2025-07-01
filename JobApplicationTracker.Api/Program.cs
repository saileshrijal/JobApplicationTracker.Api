using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Interface;
using JobApplicationTracker.Api.Data.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//Register services
builder.Services.AddScoped<IJobTypeService, JobTypeService>();
builder.Services.AddScoped<IAdminActionService, AdminActionService>();
builder.Services.AddScoped<IAdminLogsService, AdminLogsService>();
builder.Services.AddScoped<ICompaniesService, CompaniesService>();
builder.Services.AddScoped<IIndustriesService, IndustriesService>();
builder.Services.AddScoped<IJobApplicationService, ApplicationsService>();
builder.Services.AddScoped<IJobApplicationStatusService, ApplicationStatusService>();
builder.Services.AddScoped<IJobSeekerExperienceService, JobSeekerExperienceService>();
builder.Services.AddScoped<IJobSeekersEducationService, JobSeekerEducationService>();
builder.Services.AddScoped<IJobSeekersService, JobSeekerService>();
builder.Services.AddScoped<IJobSeekersSkillsService, JobSeekerSkillsService>();
builder.Services.AddScoped<IJobsService, JobsService>();
builder.Services.AddScoped<INotificationsService, NotificationsService>();
builder.Services.AddScoped<INotificationsTypesService, NotificationTypesService>();
builder.Services.AddScoped<ISkillsService, SkillsService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUsersTypeService, UserTypesService>();

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