using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Task19
{
    public class ServerThread : IDisposable
    {
        private readonly BlockingCollection<ICommand> _queue = new();
        private readonly CancellationTokenSource _cts = new();
        private readonly Thread _worker;
        private readonly IScheduler _scheduler;

        public ServerThread(IScheduler scheduler)
        {
            _scheduler = scheduler;
            _worker = new Thread(WorkLoop) { IsBackground = true };
            _worker.Start();
        }

        public void Enqueue(ICommand cmd) => _queue.Add(cmd);
        public void Stop() => _cts.Cancel();
        public void Wait() => _worker.Join();

        private void WorkLoop()
        {
            try
            {
                while (true)
                {
                    ICommand cmd;
                    if (_queue.TryTake(out var queued))
                        cmd = queued;
                    else if (_scheduler.HasCommand())
                        cmd = _scheduler.Select();
                    else
                        cmd = _queue.Take(_cts.Token);

                    cmd.Execute();

                    if (cmd is ILongRunningCommand longCmd && !longCmd.IsCompleted)
                        _scheduler.Add(longCmd);
                }
            }
            catch (OperationCanceledException) { }
        }

        public void Dispose() => Stop();
    }
}
