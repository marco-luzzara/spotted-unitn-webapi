using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SpottedUnitn.Data.Test.Attributes
{
    public class InMemoryContextDataSourceAttribute : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            yield return new object[] {
                new DbContextOptionsBuilder<ModelContext>().UseSqlite(connection)
            };
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (data != null)
                return $"Method {methodInfo.Name} - Context: InMemory";

            return null;
        }
    }
}
