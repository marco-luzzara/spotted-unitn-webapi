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
using Microsoft.AspNetCore.Authorization;
using SpottedUnitn.WebApi.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpottedUnitn.WebApi.Controllers
{
    [Route("shops")]
    [Authorize]
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

        // GET shops
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ShopBasicInfoDto>>> GetShops()
        {
            return await this.shopService.GetShopsAsync();
        }

        // POST shops
        [HttpPost]
        [Authorize(Policy = AuthorizationOptionsExtension.onlyAdminPolicy)]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        public async Task CreateShop([FromForm] ShopDataDto shop)
        {
            await this.shopService.AddShopAsync(shop);
        }

        // GET shops/5
        [HttpGet("{shopId}")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shop>> GetShop(int shopId)
        {
            return await this.shopService.GetShopInfoAsync(shopId);
        }

        // PUT shops/5
        [HttpPut("{shopId}")]
        [Authorize(Policy = AuthorizationOptionsExtension.onlyAdminPolicy)]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task ModifyShop(int shopId, [FromForm] ShopDataDto shop)
        {
            await this.shopService.ChangeShopDataAsync(shopId, shop);
        }

        // DELETE shops/5
        [HttpDelete("{shopId}")]
        [Authorize(Policy = AuthorizationOptionsExtension.onlyAdminPolicy)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task DeleteShop(int shopId)
        {
            await this.shopService.DeleteShopAsync(shopId);
        }

        // GET shops/5/coverPicture
        [HttpGet("{shopId}/coverPicture")]
        [AllowAnonymous]
        [Produces("application/octet-stream")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult<byte[]>> GetShopCoverPicture(int shopId)
        {
            return await this.shopService.GetCoverPictureAsync(shopId);
        }
    }
}
