using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace RAKBANK.Models
{
    [ContentType(DisplayName = "Product Listing Block",
      GUID = "75b8bc8d-2430-42e5-9eb9-6dead9a06ab7",
      Description = "Product Listing Block in which multiple products items will get dragged down",
      GroupName = SystemTabNames.Content)]
       [AvailableContentTypes(Include = new[] { typeof(ProductItemBlock)})]
    public class ProductsListingBlock : BlockData
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual required string DisplayName { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual required string Description { get; set; }

        [Display(Name = "Product Child Components",
            Description = "Insert Product childs here"
            , Order = 30)]
        [UIHint(UIHint.Block)]
        [AllowedTypes(AllowedTypes = new[] { typeof(ProductItemBlock) })]
        public virtual required ContentArea ProductArea { get; set; }

    }
    public class ProductListingViewModel
    {
        public ProductListingViewModel()
        {
            childProducts = [];
        }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<ProductItemViewModel> childProducts { get; set; }
    }
}
