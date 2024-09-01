using EPiServer.Framework.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace RAKBANK.Features.Media
{
	[ContentType(
		DisplayName = "GenericMedia",
		GUID = "5FC85557-E455-4B89-BA6E-863B7EEDF329",
		Description = "")]
	[MediaDescriptor(ExtensionString = "pdf,doc,docx,txt")]
	public class GenericMedia : MediaData
	{
		[CultureSpecific]
		[Display(
			Name = "MyProperty",
			Description = "My property description",
			GroupName = SystemTabNames.Content,
			Order = 10)]
		public virtual string Description { get; set; }
	}
}
