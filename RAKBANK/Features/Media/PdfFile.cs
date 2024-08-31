﻿using EPiServer.Framework.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace ADGM_Migration.Features.Media
{
	[ContentType(DisplayName = "Portable Document Format",
		// your code will have a GUID
		Description = "Use this to upload Portable Document Format (PDF) files.")]
	[MediaDescriptor(ExtensionString = "pdf")]
	public class PdfFile : MediaData
	{
		[Display(Name = "Render preview image")]
		// false: render as simple hyperlink
		// true: render as clickable thumbnail preview image
		public virtual bool RenderPreviewImage { get; set; }
	}
}