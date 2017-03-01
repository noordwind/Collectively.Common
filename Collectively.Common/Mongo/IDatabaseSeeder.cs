using System.Threading.Tasks;

namespace Collectively.Common.Mongo
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}