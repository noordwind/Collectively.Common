using Collectively.Messages.Events;

namespace Collectively.Common.Services
{
    public interface IResourceFactory
    {
         Resource Resolve<T>(params object[] args) where T : class;
    }
}