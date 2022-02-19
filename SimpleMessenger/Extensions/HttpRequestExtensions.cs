using Microsoft.AspNetCore.Http;

namespace SimpleMessenger.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjax(this HttpRequest request)
            => request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}
