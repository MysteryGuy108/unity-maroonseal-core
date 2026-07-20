using System.Collections.Generic;
using UnityEngine;

namespace MaroonSeal
{
    public class CommandInvoker<TCommand> : ICommandInvoker<TCommand> where TCommand : ICommand
    {
        private List<TCommand> commands;
        private int maxCount;

        #region Constructor
        public CommandInvoker(int _maxCount = int.MaxValue)
        {
            commands = new();
            maxCount = Mathf.Max(_maxCount, 0); 
        }
        #endregion

        public void ExecuteCommand(TCommand _command)
        {
            _command.Execute();
            commands.Add(_command);
            if (commands.Count >= maxCount) { Dequeue(); }
        }

        public TCommand Pop()
        {
            if (commands.Count <= 0) { return default; }
            TCommand command = commands[^1];
            commands.RemoveAt(commands.Count-1);
            return command;
        }

        public TCommand Dequeue()
        {
            if (commands.Count <= 0) { return default; }
            TCommand command = commands[0];
            commands.RemoveAt(0);
            return command;
        } 
    }
}
