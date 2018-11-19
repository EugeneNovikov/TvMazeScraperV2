using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shared.Exceptions;

namespace Shared.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static HttpResponseMessage UnwindHttpExceptions(this HttpResponseMessage httpResponseMessage)
        {
            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.TooManyRequests:
                    throw new TooManyRequestsException();
                case HttpStatusCode.InternalServerError:
                    throw new InternalServerErrorException();
                default:
                    return httpResponseMessage;
            }
        }

        public static async Task<string> ReadContentAsStringAsync(this HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage?.Content == null)
            {
                throw new InvalidOperationException(nameof(httpResponseMessage.Content));
            }

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}