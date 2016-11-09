namespace Coolector.Common.Events
{
    public interface IRejectedEvent : IAuthenticatedEvent
    {
        string Reason { get; }
    }
}