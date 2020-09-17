using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpottedUnitn.Data.Dto.Shop;
using SpottedUnitn.Services.Interfaces;
using SpottedUnitn.Services.Dto.Shop;
using SpottedUnitn.Model.ShopAggregate;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpottedUnitn.WebApi.Controllers
{
    [Route("shops")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        protected IShopService shopService;
        protected ILogger<ShopsController> logger;

        public ShopsController(ILogger<ShopsController> logger, IShopService shopService)
        {
            this.shopService = shopService;
            this.logger = logger;
        }

        // GET: shops
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ShopBasicInfoDto>>> GetShops()
        {
            return null;
        }

        // POST shops
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        public async Task CreateShop([FromBody] ShopCreationDto shop)
        {
        }

        // GET shops/5
        [HttpGet("{shopId}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shop>> GetShop(int shopId)
        {
            return null;
        }

        // PUT shops/5
        [HttpPut("{shopId}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task ModifyShop(int shopId, [FromBody] Shop shop)
        {
        }

        // DELETE shops/5
        [HttpDelete("{shopId}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task DeleteShop(int shopId)
        {
        }

        // GET shops/5
        [HttpGet("{shopId}/coverPicture")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult<byte[]>> GetShopCoverPicture(int shopId)
        {
            return null;
        }
    }
}
