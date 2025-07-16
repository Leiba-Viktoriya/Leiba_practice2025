using System.Threading;

namespace Task18
{
    internal class InitThreadIdCommand : ICommand
    {
        private readonly ServerThread _thread;
        private readonly ManualResetEventSlim _started;

        public InitThreadIdCommand(ServerThread thread, ManualResetEventSlim started)
        {
            _thread = thread;
            _started = started;
        }

        public void Execute()
        {
            _thread.ManagedThreadId = Thread.CurrentThread.ManagedThreadId;
            _started.Set();
        }
    }
}
