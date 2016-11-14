namespace Coolector.Common.Commands.Users
{
    public class Register : ICommand
    {
        public Request Request { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}