using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Interface;
using JobApplicationTracker.Api.Data.Service;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// authentication service
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("jwtSettings").Get<JwtSettings>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateActor = true,
            ValidateIssuer = true,
            ValidAudience = jwtSettings?.Audience,
            ValidIssuer = jwtSettings?.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key ?? "")),
            ValidateIssuerSigningKey = true,

            ClockSkew = TimeSpan.Zero,
        };

    })
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.AccessDeniedPath = "/";
        options.LogoutPath = "/";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.IsEssential = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

//Register services
builder.Services.AddScoped<IJobTypeService, JobTypeService>();
builder.Services.AddScoped<IAdminActionService, AdminActionService>();
builder.Services.AddScoped<IAdminLogsService, AdminLogsService>();
builder.Services.AddScoped<ICompaniesService, CompaniesService>();
builder.Services.AddScoped<ICompanySizesService, CompanySizeService>();
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
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddScoped<ICookieService,CookieService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();


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