using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SpottedUnitn.Data.Test.Attributes
{
    public class DbContextDataSourceAttribute : Attribute, ITestDataSource
    {
        protected readonly IConfiguration configs = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            yield return new object[] {
                new DbContextOptionsBuilder<ModelContext>().UseSqlServer(this.configs.GetConnectionString("UnitnSpotted"))
            };
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (data != null)
                return $"Method {methodInfo.Name} - Context: SqlServer";

            return null;
        }
    }
}
