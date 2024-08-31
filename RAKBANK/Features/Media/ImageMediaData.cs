using EPiServer.Framework.DataAnnotations;

namespace ADGM_Migration.Features.Media
{
	[ContentType(DisplayName = "Image File",
			GUID = "20644be7-3ca1-4f84-b893-ee021b73ce6c",
			Description = "Used for image file types such as jpg, jpeg, jpe, ico, gif, bmp, png, svg")]
	[MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png,svg")]
	public class ImageMediaData : ImageData
	{
	}
}
