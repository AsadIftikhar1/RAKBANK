using EPiServer.Web.Routing;
using RAKBANK.Extensions;
using RAKBANK.Models;
using System.Globalization;

namespace RAKBANK.services
{
    public class ProductService
    {
        private readonly Dictionary<ContentReference, ProductListingViewModel> _createdProductListingViewModel = new();
        private readonly Dictionary<ContentReference, ProductItemViewModel> _createdProductItemViewModel = new();
        private readonly UrlResolver _urlResolver;
        public ProductService(UrlResolver urlResolver)
        {
            _urlResolver = urlResolver;
        }
        #region Product Listing 
        public List<ProductListingViewModel> BuildProductListTab(ContentArea contentArea,
              CultureInfo language,
              IContentRepository _contentRepository)
        {

            var listItems = contentArea.GetContentsFromContenArea<ProductsListingBlock>(language, _contentRepository)
                .Select(m =>
                {
                    var initAncestor = new List<ContentReference>();
                    return BuildProductList(m, initAncestor, language, _contentRepository);
                });

            return listItems.ToList();
        }


        public ProductListingViewModel BuildProductList(ProductsListingBlock itemBlock,
            List<ContentReference> ancestors,
            CultureInfo language,
            IContentRepository _contentRepository)
        {
            var contentRef = (itemBlock as IContent).ContentLink;

            if (_createdProductListingViewModel.ContainsKey(contentRef))
            {
                return _createdProductListingViewModel[contentRef];
            }

            var itemModel = new ProductListingViewModel()
            {
                DisplayName = itemBlock.DisplayName,
                Description = itemBlock.Description,
                childProducts = BuildProductItemsTab(itemBlock.ProductArea, language,_contentRepository)
            };
            _createdProductListingViewModel.Add(contentRef, itemModel);

            return itemModel;
        }
        #endregion

        #region Product Ites
        public List<ProductItemViewModel> BuildProductItemsTab(ContentArea contentArea,
            CultureInfo language,
            IContentRepository _contentRepository)
        {

            var listItems = contentArea.GetContentsFromContenArea<ProductItemBlock>(language, _contentRepository)
                .Select(m =>
                {
                    var initAncestor = new List<ContentReference>();
                    return BuildProductItems(m, initAncestor, language, _contentRepository);
                });

            return listItems.ToList();
        }

        public ProductItemViewModel BuildProductItems(ProductItemBlock itemBlock,
          List<ContentReference> ancestors,
          CultureInfo language,
          IContentRepository _contentRepository)
        {
            var contentRef = (itemBlock as IContent).ContentLink;

            if (_createdProductItemViewModel.ContainsKey(contentRef))
            {
                return _createdProductItemViewModel[contentRef];
            }

            var itemModel = new ProductItemViewModel()
            {
                id= contentRef.ID.ToString(),
                DisplayName = itemBlock.DisplayName,
                Description = itemBlock.Description,
                image = _urlResolver.GetUrl(itemBlock.image),
                price=itemBlock.price
            };
            _createdProductItemViewModel.Add(contentRef, itemModel);

            return itemModel;
        }
        #endregion
    }
}
