namespace Collectively.Common.Queries
{
    public interface IAuthenticatedQuery : IQuery
    {
        string UserId { get; set; }
    }
}