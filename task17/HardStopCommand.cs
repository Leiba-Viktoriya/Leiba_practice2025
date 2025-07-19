using System;
using System.Threading;

namespace Task17
{
    public class HardStopCommand : ICommand
    {
        private readonly ServerThread _thread;

        public HardStopCommand(ServerThread thread)
        {
            _thread = thread;
        }

        public void Execute()
        {
            if (Thread.CurrentThread.ManagedThreadId != _thread.ManagedThreadId)
                throw new InvalidOperationException();
            _thread.RequestHardStop();
        }
    }
}
