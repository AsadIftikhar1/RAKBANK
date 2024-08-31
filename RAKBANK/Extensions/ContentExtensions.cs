using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using EPiServer.Web.Routing;
using System.Globalization;
using EPiServer.Framework.Blobs;

namespace RAKBANK.Extensions
{
    public static class ContentExtensions
    {
        private static readonly Injected<IContentLoader> ContentLoader;
        private static readonly Injected<IUrlResolver> UrlResolver;

        public static string ToFriendlyUrl(this Url internalUrl)
        {
            if (internalUrl == null)
            {
                return string.Empty;
            }

            var url = new UrlBuilder(internalUrl);


            return UrlResolver.Service.GetUrl(url, ContextMode.Default).TrimEnd('/') ?? string.Empty;
        }

        public static string ToFriendlyUrl(this ContentReference contentReference, string language = null)
        {
            return ContentReference.IsNullOrEmpty(contentReference)
                ? string.Empty
                : UrlResolver.Service.GetUrl(contentReference, language).TrimEnd('/');
        }


        public static string ToFriendlyUrl(this PageData pageData)
        {
            if (pageData == null)
            {
                return string.Empty;
            }

            return UrlResolver.Service.GetUrl(pageData).TrimEnd('/');
        }

        public static string ToFriendlyUrl(this LinkItem linkItem)
        {
            if (linkItem == null)
            {
                return string.Empty;
            }

            var url = new Url(linkItem.Href);
            var urlBuilder = new UrlBuilder(url);
            return UrlResolver.Service.GetUrl(urlBuilder, ContextMode.Default).TrimEnd('/');
        }

        public static List<T> GetContentsFromContenArea<T>(this ContentArea contentArea, CultureInfo language, IContentRepository contentRepository = null) where T : IContentData
        {
            contentRepository ??= ServiceLocator.Current.GetInstance<IContentRepository>();
            var subItemsLink = GetContentReferencesOfContentArea(contentArea);

            var items = contentRepository.GetItems(subItemsLink, language)
                .OfType<T>().ToList();
            return items;
        }

        public static List<T> GetContentsFromContenArea<T>(this ContentArea contentArea, CultureInfo language, IContentLoader contentLoader) where T : IContentData
        {
            contentLoader ??= ServiceLocator.Current.GetInstance<IContentLoader>();
            var subItemsLink = GetContentReferencesOfContentArea(contentArea);

            var items = contentLoader.GetItems(subItemsLink, language)
                .OfType<T>().ToList();
            return items;
        }

		public static List<ContentReference> GetContentReferencesOfContentArea(this ContentArea contentArea)
		{
			return contentArea
				?.FilteredItems
				?.Select(m => m.ContentLink).ToList() ?? new List<ContentReference>(0);
		}
	
    
        public static void AddNonExistedItemToList<T>(List<T> pageList, IEnumerable<T> addedList) where T : IContent
        {
            pageList.AddRange(addedList
                .Where(page1 => pageList.All(page2 => !IsEqualPageData(page1, page2))));
        }

        public static bool IsEqualPageData(IContent page1, IContent page2)
        {
            return page1.ContentLink.ID == page2.ContentLink.ID && (page1 as ILocalizable)?.Language.Name == (page2 as ILocalizable)?.Language.Name;
        }
        public static string DownloadUrl(this ContentReference mediaReference)
        {
            if (ContentReference.IsNullOrEmpty(mediaReference)
                || !ContentLoader.Service.TryGet(mediaReference, out MediaData mediaData)) return string.Empty;

            if ((mediaData is IBinaryStorable binaryStorable ? binaryStorable.BinaryData : (Blob)null) == null)
                return string.Empty;

            string url = UrlResolver.Service.GetUrl(mediaData);
            if (string.IsNullOrEmpty(url))
                return string.Empty;
            UrlBuilder urlBuilder = new UrlBuilder(url);
            urlBuilder.Path = $"{urlBuilder.Path}/{RoutingConstants.DownloadSegment}";
            return urlBuilder.ToString();
        }
    }
}
