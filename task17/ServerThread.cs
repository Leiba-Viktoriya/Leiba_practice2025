using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Task17
{
    public class ServerThread : IDisposable
    {
        private readonly BlockingCollection<ICommand> _queue = new();
        private readonly CancellationTokenSource _cts = new();
        private readonly Thread _worker;

        public int ManagedThreadId { get; internal set; }

        public ServerThread()
        {
            _worker = new Thread(WorkLoop) { IsBackground = true };
            _worker.Start();
            var started = new ManualResetEventSlim();
            Enqueue(new InitThreadIdCommand(this, started));
            started.Wait();
        }

        public void Enqueue(ICommand cmd) => _queue.Add(cmd);
        public void RequestSoftStop() => _queue.CompleteAdding();
        public void RequestHardStop() => _cts.Cancel();
        public void WaitForCompletion() => _worker.Join();

        private void WorkLoop()
        {
            ManagedThreadId = Thread.CurrentThread.ManagedThreadId;
            try
            {
                foreach (var cmd in _queue.GetConsumingEnumerable(_cts.Token))
                {
                    try { cmd.Execute(); } catch { }
                }
            }
            catch (OperationCanceledException) { }
        }

        public void Dispose()
        {
            RequestHardStop();
            _worker.Join();
            _queue.Dispose();
            _cts.Dispose();
        }
    }
}
