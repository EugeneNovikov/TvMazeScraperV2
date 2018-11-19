using System;

namespace Shared.Services
{
    public class UrlFormatService : IUrlFormatService
    {
        public string FormatUrlComponent(string urlComponent, params object[] urlParameters)
        {
            if (string.IsNullOrEmpty(urlComponent))
            {
                throw new ArgumentNullException(nameof(urlComponent));
            }

            return string.Format(urlComponent, urlParameters);
            
        }
    }
}