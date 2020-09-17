using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Services.Dto.Shop
{
    public class ShopCreationDto
    {
        public string Name { get; set}

        public string LinkToSite { get; set; }
        
        public string Description { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public byte[] CoverPicture { get; set; }
        
        public string Discount { get; set; }
        
        public string PhoneNumber { get; set; }
    }
}
