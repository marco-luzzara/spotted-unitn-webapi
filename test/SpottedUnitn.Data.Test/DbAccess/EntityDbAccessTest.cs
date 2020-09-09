using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Data.Test.DbAccess
{
    public class EntityDbAccessTest
    {
        protected readonly IConfiguration configs = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        protected static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected ModelContext GetModelContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ModelContext>()
               .UseSqlServer(this.configs.GetConnectionString("UnitnSpotted"))
               .UseLoggerFactory(loggerFactory)
               .EnableDetailedErrors();

            return new ModelContext(optionsBuilder.Options);
        }
    }
}
