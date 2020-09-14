using SpottedUnitn.Data.DbAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Data.Test.DbAccessTest
{
    public class UT_ShopDbAccessTest : EntityDbAccessTest
    {
        protected ShopDbAccess GetDbAccessInstance(ModelContext ctx)
        {
            var dbAccess = new ShopDbAccess(ctx, this.dtoService);
            return dbAccess;
        }
    }
}
