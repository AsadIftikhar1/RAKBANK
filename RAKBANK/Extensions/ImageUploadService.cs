using EPiServer.Core.Internal;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework.Blobs;
using EPiServer.Security;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ADGM_Migration.Features.Media;
using EPiServer.ServiceLocation;

namespace RAKBANK.Extensions
{
    public static class ImageUploadService
    {
        public static ContentReference UploadImageFromUrl(byte[] data,string fileName,string fileextension ,ContentReference imageFolderReference)
        {
            dynamic imageContentRef = (dynamic)null;
            try
            {
                var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
                var blobFactory = ServiceLocator.Current.GetInstance<IBlobFactory>();
                var contentAssetHelper = ServiceLocator.Current.GetInstance<ContentAssetHelper>();
                var imageFile = contentRepository.GetDefault<ImageMediaData>(contentAssetHelper.GetOrCreateAssetFolder(imageFolderReference).ContentLink);
                imageFile.Name = fileName;

                var blob = blobFactory.CreateBlob(imageFile.BinaryDataContainer, fileextension);
                using (var s = blob.OpenWrite())
                {
                    var w = new StreamWriter(s);
                    w.BaseStream.Write(data, 0, data.Length);
                    w.Flush();
                }
                imageFile.BinaryData = blob;
                imageContentRef = contentRepository.Save(imageFile, SaveAction.Publish,AccessLevel.NoAccess);
            }
            catch (Exception ex)
            {
                Console.WriteLine("",ex.Message);
            }

            return imageContentRef;
        }
        public static byte[] ConvertToByteArrayAsync(this IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new byte[0];
            }

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public static string GetFileExtension(this IFormFile file)
        {
            if (file == null || string.IsNullOrWhiteSpace(file.FileName))
            {
                return string.Empty;
            }

            return Path.GetExtension(file.FileName);
        }
    }
}
