using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Task18
{
    public class ServerThread : IDisposable
    {
        private readonly BlockingCollection<ICommand> _queue = new();
        private readonly CancellationTokenSource _cts = new();
        private readonly Thread _worker;
        private readonly IScheduler _scheduler;

        public int ManagedThreadId { get; internal set; }

        public ServerThread(IScheduler scheduler)
        {
            _scheduler = scheduler;
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
            try
            {
                while (true)
                {
                    ICommand cmd = _scheduler.HasCommand()
                        ? _scheduler.Select()
                        : _queue.Take(_cts.Token);

                    try
                    {
                        cmd.Execute();
                        if (cmd is ILongRunningCommand longCmd && !longCmd.IsCompleted)
                            _scheduler.Add(longCmd);
                    }
                    catch { }
                }
            }
            catch (OperationCanceledException) { }
            catch (InvalidOperationException) { }
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
