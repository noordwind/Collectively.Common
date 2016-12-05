namespace Coolector.Common.Commands
{
    public interface ICommand
    {
        Request Request { get; set; }
    }
}