using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace APIExplorer.ApiExplorer
{
    /// <summary>
    /// Main configuration object for the API Explorer
    /// </summary>
    public class ApiExplorerOptions
    {
        private string? _documentName = null;
        public string ApplicationName { get; set; } = "Optimizely CMS 12";
        public Uri? MasterUrl { get; set; } // = new Uri("http://localhost:8000");
        public string ApplicationVersion { get; set; } = "v3.0";
        public static ApiExplorerOptions CreateFromSiteURL(string siteUrl) => new() { MasterUrl = new Uri(siteUrl) };
        public static ApiExplorerOptions CreateFromSiteURL(Uri siteUrl) => new() { MasterUrl = siteUrl };
        public string DocumentName
        {
            get
            {
                return _documentName ?? ApplicationVersion;
            }
            set
            {
                _documentName = value;
            }
        }
        public OpenApiInfo CreateApiInfo()
        {
            var info = new OpenApiInfo { Title = $"{ApplicationName} - {ApplicationVersion}", Version = ApplicationVersion };
            ConfigureInfo?.Invoke(info);
            return info;
        }
        public Action<OpenApiInfo>? ConfigureInfo { get; set; } = null;

    }
}
