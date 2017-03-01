using System.Threading.Tasks;

namespace Collectively.Common.Mongo
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}