using System;
using System.Collections.Generic;

namespace Task19
{
    public class RoundRobinScheduler : IScheduler
    {
        private readonly List<ICommand> _commands = new();
        private int _current;
        private readonly object _lock = new();

        public bool HasCommand()
        {
            lock (_lock)
                return _commands.Count > 0;
        }

        public ICommand Select()
        {
            lock (_lock)
            {
                if (_commands.Count == 0)
                    throw new InvalidOperationException();

                if (_current >= _commands.Count)
                    _current = 0;

                var cmd = _commands[_current];
                _commands.RemoveAt(_current);
                if (_commands.Count > 0)
                    _current %= _commands.Count;
                else
                    _current = 0;

                return cmd;
            }
        }

        public void Add(ICommand cmd)
        {
            lock (_lock)
                _commands.Add(cmd);
        }
    }
}
