﻿using SpottedUnitn.Model.ShopAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Data.DbAccess
{
    public class ShopDbAccess : EntityDbAccess<Shop>, IShopDbAccess
    {
        public ShopDbAccess(ModelContext modelContext) : base(modelContext)
        {
        }
    }
}
