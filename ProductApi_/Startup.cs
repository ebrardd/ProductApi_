
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using NuGet.Configuration;
using ProductApi_.Configs;
using ProductApi_.Services;
using Swashbuckle.AspNetCore;


namespace ProductApi_
{
    public class Startup
    {
        private readonly string _environment;
        private readonly Settings _settings;

        public Startup(IWebHostEnvironment env)
        {
            Console.WriteLine($"Environment: {env.EnvironmentName}");
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
            var envVariables = Environment.GetEnvironmentVariables();
            if (string.IsNullOrWhiteSpace(envVariables["ASPNETCORE_ENVIRONMENT"]?.ToString()))
                throw new ArgumentNullException("ASPNETCORE_ENVIRONMENT");
            _environment = envVariables["ASPNETCORE_ENVIRONMENT"].ToString();
            _settings = Configuration.GetSection("Settings").Get<Settings>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => // Cross Origin Source
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                .WithOrigins("*")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
                });
            });
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSingleton(_settings);
            services.AddResponseCompression();
            services.AddSingleton<IService, Service>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseCors("AllowAll");
            app.UseHttpsRedirection();//
            app.UseStaticFiles();
            app.UseAuthorization();//
            app.UseResponseCompression();
            app.UseRouting();
            app.UseSwaggerUI();
            app.UseSwagger();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}