using System.ComponentModel.DataAnnotations;

namespace RAKBANK.Models.Pages
{

    [ContentType(
            DisplayName = "Rak Bank Start Page",
            GUID = "88844029-4be3-4480-9b4c-e32afe037abf",
            Description = "")]
    public class StartPage : PageData
    {
        [Display(Name = "Content Area for Product Listing",
        Description = "Content Area for Product Listing",
        GroupName = SystemTabNames.Content, Order = 1000)]
        public virtual ContentArea ProductListingArea { get; set; }
    }
}
