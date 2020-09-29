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
using SpottedUnitn.WebApi.ErrorHandling;
using SpottedUnitn.Model.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpottedUnitn.WebApi.Controllers
{
    [Route("shops")]
    [Authorize]
    [ApiController]
    public class ShopsController : EntityController
    {
        protected IShopService shopService;
        protected ILogger<ShopsController> logger;

        public ShopsController(ILogger<ShopsController> logger, IShopService shopService, ICustomExceptionHandler excHandler) : base(excHandler)
        {
            this.shopService = shopService;
            this.logger = logger;
        }

        // GET shops
        /// <summary>
        /// get all shops
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ShopBasicInfoDto>>> GetShops()
        {
            return await this.shopService.GetShopsAsync();
        }

        // POST shops
        /// <summary>
        /// add new shop
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = AuthorizationOptionsExtension.onlyAdminPolicy)]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> CreateShop([FromForm] ShopDataDto shop)
        {
            try
            {
                await this.shopService.AddShopAsync(shop);
                return Ok();
            }
            catch (ShopException exc) when (exc.HasCodeIn(
                (int)ShopException.ShopExceptionCode.InvalidName,
                (int)ShopException.ShopExceptionCode.InvalidDescription,
                (int)ShopException.ShopExceptionCode.InvalidPhoneNumber,
                (int)ShopException.ShopExceptionCode.InvalidDiscount,
                (int)ShopException.ShopExceptionCode.InvalidCoverPicture,
                (int)ShopException.ShopExceptionCode.InvalidLinkToSite,
                (int)ShopException.ShopExceptionCode.InvalidLocationAddress,
                (int)ShopException.ShopExceptionCode.InvalidLocationCity,
                (int)ShopException.ShopExceptionCode.InvalidLocationProvince,
                (int)ShopException.ShopExceptionCode.InvalidLocationPostalCode,
                (int)ShopException.ShopExceptionCode.InvalidLocationLatitude,
                (int)ShopException.ShopExceptionCode.InvalidLocationLongitude))
            {
                return BadRequest(exc);
            }
        }

        // GET shops/5
        /// <summary>
        /// get shop from shopId
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpGet("{shopId}")]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shop>> GetShop(int shopId)
        {
            try
            {
                return await this.shopService.GetShopInfoAsync(shopId);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.ShopIdNotFound)
            {
                return NotFound(exc);
            }
        }

        // PUT shops/5
        /// <summary>
        /// change shop data
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="shop"></param>
        /// <returns></returns>
        [HttpPut("{shopId}")]
        [Authorize(Policy = AuthorizationOptionsExtension.onlyAdminPolicy)]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ModifyShop(int shopId, [FromForm] ShopDataDto shop)
        {
            try
            {
                await this.shopService.ChangeShopDataAsync(shopId, shop);
                return Ok();
            }
            catch (ShopException exc) when (exc.HasCodeIn(
                (int)ShopException.ShopExceptionCode.InvalidName,
                (int)ShopException.ShopExceptionCode.InvalidDescription,
                (int)ShopException.ShopExceptionCode.InvalidPhoneNumber,
                (int)ShopException.ShopExceptionCode.InvalidDiscount,
                (int)ShopException.ShopExceptionCode.InvalidCoverPicture,
                (int)ShopException.ShopExceptionCode.InvalidLinkToSite,
                (int)ShopException.ShopExceptionCode.InvalidLocationAddress,
                (int)ShopException.ShopExceptionCode.InvalidLocationCity,
                (int)ShopException.ShopExceptionCode.InvalidLocationProvince,
                (int)ShopException.ShopExceptionCode.InvalidLocationPostalCode,
                (int)ShopException.ShopExceptionCode.InvalidLocationLatitude,
                (int)ShopException.ShopExceptionCode.InvalidLocationLongitude))
            {
                return BadRequest(exc);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.ShopIdNotFound)
            {
                return NotFound(exc);
            }
        }

        // DELETE shops/5
        /// <summary>
        /// delete specified shop
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpDelete("{shopId}")]
        [Authorize(Policy = AuthorizationOptionsExtension.onlyAdminPolicy)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteShop(int shopId)
        {
            try
            {
                await this.shopService.DeleteShopAsync(shopId);
                return Ok();
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.ShopIdNotFound)
            {
                return NotFound(exc);
            }
        }

        // GET shops/5/coverPicture
        /// <summary>
        /// get shop cover picture
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpGet("{shopId}/coverPicture")]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Octet)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetShopCoverPicture(int shopId)
        {
            try
            {
                var data = await this.shopService.GetCoverPictureAsync(shopId);
                return new FileContentResult(data, MediaTypeNames.Application.Octet);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.ShopIdNotFound)
            {
                return NotFound(exc);
            }
        }
    }
}
