using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace RAKBANK.Models
{
    [ContentType(DisplayName = "Product Items",
      GUID = "30685434-33DE-42AF-88A7-3126B936AEAD",
      Description = "Single Block Items",
      GroupName = SystemTabNames.Content)]
    public class ProductItemBlock : BlockData
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual required string DisplayName { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual required string Description { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 40)]
        public virtual required string price { get; set; }

        [Display(Name = "Image",
            Description = "Insert Image Url here", Order = 30)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference image { get; set; }
    }
    public class ProductItemViewModel
    {
        public string id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string image { get; set; }
        public string price { get; set; }
    }
    public class ProductRequestDto
    {
        public string? id { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? price { get; set; }
    }
}
