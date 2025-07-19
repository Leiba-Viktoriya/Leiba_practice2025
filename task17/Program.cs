using System;
using System.Threading;
using Task17;

namespace Task17
{
    class Program
    {
        static void Main()
        {
            using var server = new ServerThread();
            server.Enqueue(new PrintCommand("A"));
            server.Enqueue(new PrintCommand("B"));
            server.Enqueue(new SoftStopCommand(server));
            server.WaitForCompletion();
            Console.WriteLine("Server stopped");
        }
    }

    class PrintCommand : ICommand
    {
        private readonly string _text;
        public PrintCommand(string text) => _text = text;
        public void Execute() => Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] {_text}");
    }
}
