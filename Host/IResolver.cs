namespace Coolector.Common.Host
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}