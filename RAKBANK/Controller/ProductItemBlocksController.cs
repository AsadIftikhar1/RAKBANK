using EPiServer.DataAccess;
using EPiServer.Globalization;
using EPiServer.Security;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;
using RAKBANK.Extensions;
using RAKBANK.Models;
using RAKBANK.Models.Pages;
using RAKBANK.services;
using SiteDefinition = EPiServer.Web.SiteDefinition;

namespace RAKBANK.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsListingController : ControllerBase
    {
        private readonly IContentLoader _contentLoader;
        private readonly IContentRepository _contentRepository;
        private readonly SiteDefinition _siteDefinition;
        private readonly ProductService _productService;
        private readonly UrlResolver _urlResolver;

        public IContentLoader Object1 { get; }
        public IContentRepository Object2 { get; }
        public EPiServer.Licensing.Services.SiteDefinition Object3 { get; }
        public ProductService Object4 { get; }
        public UrlResolver Object5 { get; }

        public ProductsListingController(IContentLoader contentLoader,
            IContentRepository contentRepository,
            SiteDefinition siteDefinition,
            ProductService productService,
            UrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _contentRepository = contentRepository;
            _siteDefinition = siteDefinition;
            _productService = productService;
            _urlResolver = urlResolver;
        }
        /// <summary>
        /// API to Retrieve all of the Product Item Blocks with in inside Product Main Block Container
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<ProductListingViewModel>> GetAllProductsListingBlocks()
        {
            dynamic productlisting = (ContentReference)null;
            IEnumerable<ProductItemBlock> productListingChildBlock = [];
            try
            {
                if (_siteDefinition.StartPage != null)
                {
                    var productsListingBlock = _contentRepository.Get<StartPage>(new ContentReference(_siteDefinition.StartPage.ID));
                    productlisting = _productService.BuildProductListTab(productsListingBlock.ProductListingArea, ContentLanguage.PreferredCulture, _contentRepository);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception message is {ex.Message} and StackTrace is {ex.StackTrace}");
            }
            return Ok(productlisting);
        }
        /// <summary>
        /// Post product into the CMS and add reference in the Product Listing Block main container
        /// </summary>
        /// <param name="p_ProductRequestDto"></param>
        /// <returns></returns>
        [HttpPut("UpdateProduct/{id}")]
        public ActionResult<ProductItemBlock> UpdateProduct([FromBody] ProductRequestDto p_ProductRequestDto)
        {
            dynamic res = (ContentReference)null;
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
                var ProductItem = _contentRepository.Get<ProductItemBlock>(new ContentReference(p_ProductRequestDto.id))
                    .CreateWritableClone() as ProductItemBlock;
                ProductItem.DisplayName = p_ProductRequestDto?.DisplayName;
                ProductItem.Description = p_ProductRequestDto?.Description;
                ProductItem.price = p_ProductRequestDto?.price;
                //ProductItem.image = p_ProductRequestDto.Image;

                var SaveProductItems = _contentRepository.Save((IContent)ProductItem, SaveAction.Publish, AccessLevel.NoAccess);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception message is {ex.Message} and StackTrace is {ex.StackTrace}");
            }
            return CreatedAtAction(nameof(UpdateProduct), new { ContentReference = res }, p_ProductRequestDto);
        }
        /// <summary>
        /// To Update an existing Product Item
        /// </summary>
        /// <param name="p_ProductRequestDto"></param>
        /// <returns></returns>
        [HttpPost("CreateProduct")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<ProductItemBlock> PostProduct(
        [FromForm] string displayName,
        [FromForm] string description,
        [FromForm] string price,
        [FromForm] IFormFile image)
        {
            dynamic res = (ContentReference)null;
            try
            {
                var bytes = ImageUploadService.ConvertToByteArrayAsync(image);
                var filename = displayName;
                var fileExtension = ImageUploadService.GetFileExtension(image);
                var Product = _contentRepository.Get<ProductsListingBlock>(new ContentReference(15)).CreateWritableClone() as ProductsListingBlock;
                var ProductItem = _contentRepository.GetDefault<ProductItemBlock>(new ContentReference(15));
                ProductItem.DisplayName = displayName;
                ProductItem.Description = description;
                ProductItem.price = price;
                var unboxObject = (IContent)ProductItem;
                unboxObject.Name = displayName;
                var SaveProductItems = _contentRepository.Save((IContent)unboxObject, SaveAction.Publish, AccessLevel.NoAccess);
                var AddblockItem = new ContentAreaItem
                {
                    ContentGuid = Guid.NewGuid(),
                    ContentLink = SaveProductItems
                };
                Product?.ProductArea.Items.Add(AddblockItem);
                var SavedProductItem = _contentRepository.Get<ProductItemBlock>(SaveProductItems).CreateWritableClone() as ProductItemBlock;
                var imageReference = ImageUploadService.UploadImageFromUrl(bytes, filename, fileExtension, SaveProductItems);
                SavedProductItem.image = imageReference;
                var addingRefernceToTheProductItemBlockOfImage = _contentRepository.Save((IContent)SavedProductItem, SaveAction.Publish, AccessLevel.NoAccess);
                var addingReferencetoTheProductListingBlock = _contentRepository.Save((IContent)Product, SaveAction.Publish, AccessLevel.NoAccess);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception message is {ex.Message} and StackTrace is {ex.StackTrace}");
            }
            return CreatedAtAction(nameof(PostProduct), new { ContentReference = res });
        }
        /// <summary>
        /// An api to delete the Product List Block
        /// </summary>
        /// <param name="p_ProductRequestDto"></param>
        /// <returns></returns>
        [HttpDelete("DeleteProduct/{id}")]
        public ActionResult<ProductItemBlock> DeleteProduct(int id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("Product is null.");
                }
                var BlockToBeDeleted = _contentLoader.Get<ProductItemBlock>(new ContentReference(id));
                dynamic BlockDeletedReference = (ContentReference)null;
                if (BlockToBeDeleted != null)
                {
                    var blockContentLink = (dynamic)BlockToBeDeleted;
                    BlockDeletedReference = (ContentReference)blockContentLink.GetType().GetProperty("ContentLink").GetValue(blockContentLink, null);
                }
                var Product = _contentRepository.Get<ProductsListingBlock>(new ContentReference(15)).CreateWritableClone() as ProductsListingBlock;
                var contentArea = Product.ProductArea;
                var itemToRemove = contentArea.Items.FirstOrDefault(x => x.ContentLink.ID == BlockDeletedReference.ID);

                Product.ProductArea.Items.Remove(itemToRemove);
                var blockReferenceRemoved = _contentRepository.Save((IContent)Product, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
                _contentRepository.Delete(BlockDeletedReference, true, AccessLevel.NoAccess);
                return Ok("The item with that id is deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception message is {ex.Message} and StackTrace is {ex.StackTrace}");
            }
            return Ok("The item with that id has not beed deleted");
        }
    }
}

