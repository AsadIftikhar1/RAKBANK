using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.ContentActionsApi.Infrastructure
{
    [Options]
    public class ContentActionApiOptions
    {
        public virtual List<PathString> ServicePathAllVersions { get; set; } = new List<PathString>()
        {
            new PathString("/api/episerver/v3.0/contentaction"),
            new PathString("/api/episerver/v2.0/contentaction")
        };
    }
}
