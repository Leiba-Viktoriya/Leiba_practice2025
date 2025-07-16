using System;
using System.Threading;

namespace Task17
{
    public class SoftStopCommand : ICommand
    {
        private readonly ServerThread _thread;

        public SoftStopCommand(ServerThread thread)
        {
            _thread = thread;
        }

        public void Execute()
        {
            if (Thread.CurrentThread.ManagedThreadId != _thread.ManagedThreadId)
                throw new InvalidOperationException();
            _thread.RequestSoftStop();
        }
    }
}
