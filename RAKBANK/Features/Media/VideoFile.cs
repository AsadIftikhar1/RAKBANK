using EPiServer.Framework.DataAnnotations;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace RAKBANK.Features.Media
{
	[ContentType(
		DisplayName = "VideoFile",
		GUID = "A6F44633-0A38-4980-8A4D-D8B66EE2E993",
		Description = "")]
	[MediaDescriptor(ExtensionString = "flv,mp4,webm")]
	public class VideoFile : VideoData
	{
		/// <summary>
		/// Gets or sets the copyright.
		/// </summary>
		public virtual string Copyright { get; set; }

		/// <summary>
		/// Gets or sets the URL to the preview image.
		/// </summary>
		[UIHint(UIHint.Image)]
		public virtual ContentReference PreviewImage { get; set; }


		public virtual bool Autoplay { get; set; }

		public virtual bool DisplayControls { get; set; }
	}
}
