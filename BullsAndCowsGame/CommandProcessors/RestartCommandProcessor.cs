﻿namespace BullsAndCows.CommandProcessors
{
    using System;
    using Contracts;

    internal class RestartCommandProcessor : CommandProcessor, ICommandProcessor
    {
        public override void ProcessCommand(string command)
        {
            if (command == "restart")
            {
                throw new NotImplementedException();
            }
            else if (this.Successor != null)
            {
                this.Successor.ProcessCommand(command);
            }
            else
            {
                throw new ArgumentNullException("There is no successor for RestartCommandProcessor.");
            }
        }
    }
}
