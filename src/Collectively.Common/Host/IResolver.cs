namespace Collectively.Common.Host
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}