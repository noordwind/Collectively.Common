using System.Threading.Tasks;

namespace Coolector.Common.Mongo
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}