using CbkSDK.Core.Event.Interface;

namespace CbkSDK.Core.Command.Interface
{
    public interface ICommand
    {
        void Execute(IEvent e = null);
        void Complete();
        void Cancel();
        CommandEvent OnCompleted { get; set; }
        CommandEvent OnCanceled { get; set; }


    }
}