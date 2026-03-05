using UnityEngine;

namespace MaroonSeal
{
    public interface ICommandInvoker<TCommand> where TCommand : ICommand
    {
        public void ExecuteCommand(TCommand _command);
    }
}
