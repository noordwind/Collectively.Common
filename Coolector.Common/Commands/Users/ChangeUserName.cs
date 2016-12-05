namespace Coolector.Common.Commands.Users
{
    public class ChangeUserName : IAuthenticatedCommand
    {
        public Request Request { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
    }
}