using System.Threading.Tasks;
using Collectively.Common.Types;

namespace Collectively.Common.Locations
{
    public interface ILocationService
    {
        Task<Maybe<LocationResponse>> GetAsync(string address);
        Task<Maybe<LocationResponse>> GetAsync(double latitude, double longitude);
    }
}