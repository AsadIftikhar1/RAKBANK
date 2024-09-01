using EPiServer.Framework.Blobs;
using EPiServer.Framework.DataAnnotations;

namespace RAKBANK.Features.Media
{
	[ContentType(DisplayName = "SVG File",
		// your code will have a GUID
		Description = "Use this to upload Scalable Vector Graphic (SVG) images.")]
	[MediaDescriptor(ExtensionString = "svg")]
	public class SvgFile : ImageData
	{
		// instead of generating a smaller bitmap file for thumbnail,
		// use the same binary vector image for thumbnail
		public override Blob Thumbnail { get => base.BinaryData; }
	}
}