namespace Task19
{
    public class HardStopCommand : ICommand
    {
        private readonly ServerThread _server;

        public HardStopCommand(ServerThread server)
        {
            _server = server;
        }

        public void Execute()
        {
            _server.Stop();
        }
    }
}
