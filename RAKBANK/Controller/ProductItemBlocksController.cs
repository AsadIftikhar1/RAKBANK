using EPiServer.DataAccess;
using EPiServer.Security;
using Microsoft.AspNetCore.Mvc;
using RAKBANK.Models;
using RAKBANK.Models.Pages;

namespace RAKBANK.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsListingController : ControllerBase
    {
        private readonly IContentLoader _contentLoader;
        private readonly IContentRepository _contentRepository;
        public ProductsListingController(IContentLoader contentLoader, IContentRepository contentRepository)
        {
            _contentLoader = contentLoader;
            _contentRepository = contentRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductsListingBlock>> GetAllProductsListingBlocks()
        {
            var productsListingBlock = _contentRepository.Get<ProductsListingBlock>(new ContentReference(6));
            int productBlockID = 0;
            if (productsListingBlock != null)
            {
                var blockContentLink = (dynamic)productsListingBlock;
                var themeblockContentLinkitem = (ContentReference)blockContentLink.GetType().GetProperty("ContentLink").GetValue(blockContentLink, null);
                productBlockID = themeblockContentLinkitem.ID;
            }
            var productListingChildBlock = _contentRepository.GetChildren<ProductItemBlock>(new ContentReference(6));
            return Ok();
        }

        [HttpPost]
        public ActionResult<ProductItemBlock> PostProduct([FromBody] ProductRequestDto p_ProductRequestDto)
        {
            try
            {
                if (p_ProductRequestDto == null)
                {
                    return BadRequest("Product is null.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var Product = _contentRepository.Get<ProductsListingBlock>(new ContentReference(15)).CreateWritableClone() as ProductsListingBlock;
                var ProductItem = _contentRepository.GetDefault<ProductItemBlock>(new ContentReference(15));
                //var result = _contentRepository.GetChildren<ProductItemBlock>(new ContentReference(23));
                ProductItem.DisplayName = p_ProductRequestDto.DisplayName;
                ProductItem.Description = p_ProductRequestDto.Description;
                ProductItem.imag = p_ProductRequestDto.Image;
                var unboxObject = (IContent)ProductItem;
                unboxObject.Name = "RT";
                var SaveProductItems = _contentRepository.Save((IContent)unboxObject, SaveAction.Publish, AccessLevel.NoAccess);
                var AddblockItem = new ContentAreaItem
                {
                    ContentGuid = Guid.NewGuid(),
                    ContentLink = SaveProductItems
                };
                Product.ProductArea.Items.Add(AddblockItem);
                 var res= _contentRepository.Save((IContent)Product, SaveAction.Publish, AccessLevel.NoAccess);

            }
            catch (Exception ex)
            {

                throw;
            }
            return CreatedAtAction(nameof(PostProduct), new { id = 1 }, p_ProductRequestDto);
        }
    }
}

