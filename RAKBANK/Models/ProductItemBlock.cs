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

        [Display(Name = "Image",
            Description = "Insert Image Url here", Order = 30)]
        public virtual required string imag { get; set; }
    }

    public class ProductRequestDto
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
