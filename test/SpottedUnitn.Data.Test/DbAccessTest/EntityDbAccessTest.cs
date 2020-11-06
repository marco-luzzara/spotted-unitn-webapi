using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SpottedUnitn.Infrastructure.Services;
using SpottedUnitn.Infrastructure.Services.FileStorage;
using SpottedUnitn.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.Test.DbAccessTest
{
    public class EntityDbAccessTest
    {
        protected IDateTimeOffsetService dtoService = new DateTimeOffsetService();
        protected IFileStorageService fileStorageService;

        protected readonly IConfiguration configs = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        protected void InitFileStorageService()
        {
            var fileStorageServiceMock = new Mock<IFileStorageService>();
            fileStorageServiceMock.Setup(s => s.StoreFileAsync(It.IsAny<string>(), It.IsAny<byte[]>(), default))
                .Returns(Task.CompletedTask);

            fileStorageServiceMock.Setup(s => s.GetFileAsync(It.IsAny<string>(), default))
                .Returns(Task.FromResult<byte[]>(null));

            fileStorageServiceMock.Setup(s => s.DeleteFileAsync(It.IsAny<string>(), default))
                .Returns(Task.CompletedTask);

            this.fileStorageService = fileStorageServiceMock.Object;
        }

        public EntityDbAccessTest()
        {
            this.InitFileStorageService();
        }

        protected void SetupFileStorageServiceMock(Action<Mock<IFileStorageService>> fssSetup)
        {
            fssSetup(Mock.Get(this.fileStorageService));
        }

        protected static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => 
        {
            builder
                .AddConsole()
                .AddLog4Net();
        });

        protected async Task<TEntity> GetObjectFromDbAsync<TEntity, TIdType>(ModelContext ctx, TIdType entityId)
            where TEntity : class, IEntity<TIdType>
        {
            var entityEntry = ctx.ChangeTracker.Entries<TEntity>().Single(s => s.Entity.Id.Equals(entityId));
            var dbProps = await entityEntry.GetDatabaseValuesAsync();
            var changedShop = (TEntity)dbProps.ToObject();
            return changedShop;
        }

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
