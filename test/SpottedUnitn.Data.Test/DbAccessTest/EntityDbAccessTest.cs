using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpottedUnitn.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Data.Test.DbAccessTest
{
    public class EntityDbAccessTest
    {
        protected IDateTimeOffsetService dtoService = new DateTimeOffsetService();
        protected readonly IConfiguration configs = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        protected static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => 
        {
            builder
                .AddConsole()
                .AddLog4Net();
        });

        protected ModelContext GetModelContext(DbContextOptionsBuilder<ModelContext> builder)
        {
            builder
               .UseLoggerFactory(loggerFactory)
               .UseLazyLoadingProxies()
               .EnableDetailedErrors();

            var ctx = new ModelContext(builder.Options);
            ctx.Database.EnsureCreated();

            return ctx;
        }
    }
}
