using System.Threading.Tasks;

namespace Coolector.Common.Mongo
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}