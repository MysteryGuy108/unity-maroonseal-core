using UnityEngine;

namespace MaroonSeal
{
    public interface ICommand
    {
        void Execute();
    }

    abstract public class CommandBase : ICommand
    {
        abstract public void Execute();
    }
}
