using SpottedUnitn.Model.ShopAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Data.Dto.Shop
{
    public class ShopInfoDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string LinkToSite { get; set; }
        
        public string Description { get; set; }
        
        public Location Location { get; set; }
        
        public string Discount { get; set; }
        
        public string PhoneNumber { get; set; }
    }
}
