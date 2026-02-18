using CVAnalyzer.Business;
using CVAnalyzer.Business.background_services;
using CVAnalyzer.Business.CV;
using CVAnalyzer.Business.CV.Interfaces;
using CVAnalyzer.Business.Interfaces;
using CVAnalyzer.Business.Vacancy;
using CVAnalyzer.Business.Vacancy.Interfaces;
using CVAnalyzer.DbLayer;
using CVAnalyzer.Mappers;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models;
using CVAnalyzer.Models.AIClient;
using CVAnalyzer.Models.HhClient;
using CVAnalyzer.Repositories;
using CVAnalyzer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;

namespace CVAnalyzer
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
            
            //ДОБАВЛЕНИЕ ЗАВИСИМОСТЕЙ В DI КОНТЕЙНЕР
            services.AddScoped<ICreateCVbyPDFCommand, CreateCVbyPDFCommand>();
            services.AddScoped<ICreateCVbyManualInputCommand, CreateCVbyManualInputCommand>();
            services.AddScoped<ICVRepository, CVRepository>();
            services.AddScoped<ICreateCVbyDocxCommand, CreateCvByDocxCommand>();
            services.AddScoped<IAnalysisResponseMapper, AnalysisResponseMapper>();
            services.AddScoped<IDbAnalysisMapper, DbAnalysisMapper>();
            services.AddScoped<IParseVacancyCommand, ParseVacancyCommand>();
            services.AddScoped<IAnalysisRepository, AnalysisRepository>();
            services.AddScoped<IPromptRepository, PromptRepository>();
                
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
            
            services.AddHostedService<AiTokenRefreshService>();
            services.AddHostedService<HhTokenRefreshService>();
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
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
