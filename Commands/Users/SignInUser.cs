namespace Coolector.Common.Commands.Users
{
    public class SignInUser : ICommand
    {
        public Request Request { get; set; }
        public string AccessToken { get; set; }
    }
}