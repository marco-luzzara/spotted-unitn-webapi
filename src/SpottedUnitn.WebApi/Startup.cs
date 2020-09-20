using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SpottedUnitn.Data;
using SpottedUnitn.Data.DbAccess;
using SpottedUnitn.Services;
using SpottedUnitn.Services.Interfaces;
using SpottedUnitn.WebApi.Configs.Options;
using SpottedUnitn.WebApi.Authorization;
using SpottedUnitn.Infrastructure.Services;

namespace SpottedUnitn.WebApi
{
    public class Startup
    {
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ModelContext>(optionsBuilder =>
                optionsBuilder
                    .UseLoggerFactory(loggerFactory)
                    .EnableDetailedErrors()
                    .UseLazyLoadingProxies()
                    .UseSqlServer(Configuration.GetConnectionString("UnitnSpotted")));

            // options
            var authSection = Configuration.GetSection("Authentication");
            services.Configure<JWTOptions>(authSection);

            // services
            services.AddSingleton<IDateTimeOffsetService, DateTimeOffsetService>();

            services.AddScoped<IUserDbAccess, UserDbAccess>();
            services.AddScoped<IShopDbAccess, ShopDbAccess>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IShopService, ShopService>();

            // configure jwt authentication
            var jwtConfigs = authSection.Get<JWTOptions>();
            var key = Encoding.ASCII.GetBytes(jwtConfigs.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddOnlyRegisteredOrAdminPolicy();
                options.AddOnlyAdminPolicy();
            });

            services.AddCors();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
