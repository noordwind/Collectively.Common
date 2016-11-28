namespace Coolector.Common.Commands.Mailing
{
    public abstract class SendEmailMessageBase : ICommand
    {
        public Request Request { get; set; }
        public string Email { get; set; }
    }
}