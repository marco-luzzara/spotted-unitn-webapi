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
using SpottedUnitn.WebApi.ErrorHandling;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

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
            services.AddTransient<IDateTimeOffsetService, DateTimeOffsetService>();
            services.AddTransient<ICustomExceptionHandler, EntityExceptionHandler>();

            services.AddScoped<IUserDbAccess, UserDbAccess>();
            services.AddScoped<IShopDbAccess, ShopDbAccess>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IShopService, ShopService>();

            #region configure jwt authentication
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
            #endregion
            services.AddAuthorization(options =>
            {
                options.AddOnlyRegisteredOrAdminPolicy();
                options.AddOnlyAdminPolicy();
            });

            #region configure localization
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("it")
                };

                opts.DefaultRequestCulture = new RequestCulture("en");
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
                opts.RequestCultureProviders = new[] { new AcceptLanguageHeaderRequestCultureProvider() };
            });
            #endregion

            services.AddCors();
            services.AddControllers();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Unitn Spotted API",
                    Description = "Api for the Spotted Unitn WebApp",
                    Contact = new OpenApiContact
                    {
                        Name = "Marco Luzzara",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/marco-luzzara/spotted-unitn-webapi"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            } 
            else
            {
                app.UseExceptionHandler("/error/production");
            }

            var localizeOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizeOptions.Value);

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
