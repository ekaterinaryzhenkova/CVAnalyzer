using CVAnalyzer.Business;
using CVAnalyzer.Business.Auth;
using CVAnalyzer.Business.Auth.Interfaces;
using CVAnalyzer.Business.background_services;
using CVAnalyzer.Business.Clients;
using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.Business.CV;
using CVAnalyzer.Business.CV.Interfaces;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using System.Text;

namespace CVAnalyzer
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private void ConfigureJwt(IServiceCollection services)
        {
            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));

            var jwtOptions = Configuration.GetSection("Jwt").Get<JwtOptions>();
            var key = Encoding.UTF8.GetBytes(jwtOptions.Key);

            services
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
        }

        
        public void ConfigureServices(IServiceCollection services)
        {
            //ДОБАВЛЕНИЕ КОНТРОЛЛЕРОВ
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
            
            //КОРСЫ
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            
            services.AddMemoryCache();
            
            //ДОБАВЛЕНИЕ ЗАВИСИМОСТЕЙ В DI КОНТЕЙНЕР
            services.AddScoped<IPasswordHasher<DbUser>, PasswordHasher<DbUser>>();
            
            services.AddScoped<ICreateCbByPdfCommand, CreateCvByPdfCommand>();
            services.AddScoped<ICreateCvByManualInputCommand, CreateCvByManualInputCommand>();
            services.AddScoped<ICreateCvByDocxCommand, CreateCvByDocxCommand>();
            services.AddScoped<IParseVacancyCommand, ParseVacancyCommand>();
            services.AddScoped<IRegisterCommand, RegisterCommand>();
            services.AddScoped<IRefreshTokenCommand, RefreshTokenCommand>();
            services.AddScoped<ILoginCommand, LoginCommand>();

            services.AddScoped<IAnalysisResponseMapper, AnalysisResponseMapper>();
            services.AddScoped<IDbAnalysisMapper, DbAnalysisMapper>();
            services.AddScoped<IDbUserMapper, DbUserMapper>();

            services.AddScoped<ICVRepository, CVRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAnalysisRepository, AnalysisRepository>();
            services.AddScoped<IPromptRepository, PromptRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            
            services.AddScoped<IPromptService, PromptService>();
            services.AddScoped<IJwtService, JwtService>();

                
            services.AddHttpClient<IAiClient, AiClient>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    //TODO: remove this and add certificate to store
                    ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
                });
            
            services.AddHttpClient<IHhClient, HhClient>(client =>
            {
                client.BaseAddress = new Uri("https://api.hh.ru/");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("CVAnalyzer/1.0 (vadivazikryzh@gmail.com)");
            });

            services.Configure<AiApiOptions>(Configuration.GetSection("AiApi"));
            services.Configure<HhApiOptions>(Configuration.GetSection("HhApi"));
            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));
            
            services.AddHostedService<AiTokenRefreshService>();
            services.AddHostedService<HhTokenRefreshService>();
            services.AddHostedService<RefreshTokenRemovalService>();
            
            services.AddSingleton<IAiTokenSettings, AiTokenSettings>();
            services.AddSingleton<IHhTokenSettings, HhTokenSettings>();
            
            //КОННЕКТ С БД
            string dbConnStr = Configuration.GetConnectionString("SqlConnectionString");
            services.AddDbContext<CVAnalyzerContext>(options =>
            {
                options.UseSqlServer(dbConnStr);
            });
            
            //HTTP КЛИЕНТ КОТОРЫЙ ИГНОРИРУЕТ СЕРТИФИКАТ ДЛЯ ПОДКЛЮЧЕНИЯ К ГИГАЧАТУ
            services.AddHttpClient("GigaChatJes")
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    //TODO: remove this and add certificate to store
                    ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
                });
            
            ConfigureJwt(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //ПОЛУЧАЕМ БД КОНТЕКСТ И ЗАПУСКАЕМ МИГРАЦИИ
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<CVAnalyzerContext>();
                db.Database.Migrate();
            }
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowReactApp");
            
            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
