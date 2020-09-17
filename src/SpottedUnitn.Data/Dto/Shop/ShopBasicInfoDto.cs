using SpottedUnitn.Model.ShopAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Data.Dto.Shop
{
    public class ShopBasicInfoDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Location Location { get; set; }

        public string Discount { get; set; }
    }
}
