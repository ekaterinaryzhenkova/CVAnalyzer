using CVAnalyzer.Business;
using CVAnalyzer.Business.Auth;
using CVAnalyzer.Business.Auth.Interfaces;
using CVAnalyzer.Business.background_services;
using CVAnalyzer.Business.Clients;
using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.Business.CV;
using CVAnalyzer.Business.CV.Interfaces;
using CVAnalyzer.Business.helpers;
using CVAnalyzer.Business.Vacancy;
using CVAnalyzer.Business.Vacancy.Interfaces;
using CVAnalyzer.DbLayer;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Mappers;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models.AIClient;
using CVAnalyzer.Models.HhClient;
using CVAnalyzer.Models.Token;
using CVAnalyzer.Repositories;
using CVAnalyzer.Repositories.Interfaces;
using CVAnalyzer.Repositories.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IPasswordHasher<DbUser>, PasswordHasher<DbUser>>();

builder.Services.AddScoped<ICreateCvByPdfCommand, CreateCvByPdfCommand>();
builder.Services.AddScoped<ICreateCvByManualInputCommand, CreateCvByManualInputCommand>();
builder.Services.AddScoped<ICreateCvByDocxCommand, CreateCvByDocxCommand>();
builder.Services.AddScoped<IParseVacancyAndCreateAnalysisCommand, ParseVacancyAndCreateAnalysisCommand>();
builder.Services.AddScoped<IRegisterCommand, RegisterCommand>();
builder.Services.AddScoped<IRefreshTokenCommand, RefreshTokenCommand>();
builder.Services.AddScoped<ILoginCommand, LoginCommand>();

builder.Services.AddScoped<IAnalysisResponseMapper, AnalysisResponseMapper>();
builder.Services.AddScoped<IDbAnalysisMapper, DbAnalysisMapper>();
builder.Services.AddScoped<IDbUserMapper, DbUserMapper>();

builder.Services.AddScoped<ICvRepository, CvRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAnalysisRepository, AnalysisRepository>();
builder.Services.AddScoped<IPromptRepository, PromptRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<IPromptService, PromptService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddSingleton<IParseCvHelper, ParseCvHelper>();

builder.Services.AddHttpClient<IAiClient, AiClient>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        //TODO: remove this and add certificate to store
        ServerCertificateCustomValidationCallback = (_, _, _, _) => true
    });

builder.Services.AddHttpClient<IHhClient, HhClient>(client =>
{
    client.BaseAddress = new Uri("https://api.hh.ru/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("CVAnalyzer/1.0 (vadivazikryzh@gmail.com)");
});

builder.Services.AddHttpClient("GigaChatJes")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        //TODO: remove this and add certificate to store
        ServerCertificateCustomValidationCallback = (_, _, _, _) => true
    });


builder.Services.Configure<AiApiOptions>(configuration.GetSection("AiApi"));
builder.Services.Configure<HhApiOptions>(configuration.GetSection("HhApi"));
builder.Services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();
var key = Encoding.UTF8.GetBytes(jwtOptions.Key);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

var dbConnStr = configuration.GetConnectionString("SqlConnectionString");

builder.Services.AddDbContext<CVAnalyzerContext>(options =>
{
    options.UseSqlServer(dbConnStr);
});

builder.Services.AddHostedService<AiTokenRefreshService>();
builder.Services.AddHostedService<HhTokenRefreshService>();
builder.Services.AddHostedService<RefreshTokenRemovalService>();

builder.Services.AddSingleton<IAiTokenSettings, AiTokenSettings>();
builder.Services.AddSingleton<IHhTokenSettings, HhTokenSettings>();


var app = builder.Build();

for (int i = 0; i < 5; i++)
{
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CVAnalyzerContext>();

        db.Database.Migrate();

        Console.WriteLine("Migrations applied");
        break;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration failed: {ex.Message}");
        Thread.Sleep(5000);
    }
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();