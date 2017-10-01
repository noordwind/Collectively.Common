using Microsoft.AspNetCore.Hosting;

namespace Collectively.Common.Host
{
    public class DefaultResolver : IResolver
    {
        private readonly IWebHost _webHost;
        public DefaultResolver(IWebHost webHost)
        {
            _webHost = webHost;
        }

        public T Resolve<T>() => (T)_webHost.Services.GetService(typeof(T));
    }
}