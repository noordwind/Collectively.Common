namespace Coolector.Common.Commands.Users
{
    public class SignUp : ICommand
    {
        public Request Request { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public string Provider { get; set; }
    }
}