using System;

namespace SmartQuant
{
    public class ConsoleEventLogger : EventLogger
    {
        public ConsoleEventLogger(Framework framework)
            : base(framework, "Console")
        {
        }

        public override void OnEvent(Event e)
        {
            throw new NotImplementedException();
        }
    }
}