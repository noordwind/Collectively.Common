using System.Threading.Tasks;
using Collectively.Common.Types;

namespace Collectively.Common.Extensions
{
    public static class MaybeAsyncExtensions
    {
        public static async Task<Result<T>> ToResult<T>(this Task<Maybe<T>> maybeTask, string errorMessage)
            where T : class
        {
            var maybe = await maybeTask;

            return maybe.ToResult(errorMessage);
        }
    }
}