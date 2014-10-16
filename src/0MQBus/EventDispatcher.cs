using System;

namespace SmartQuant
{
    public class EventDispatcher
    {
        protected internal Framework framework;

        public event FrameworkEventHandler FrameworkCleared;

        public EventDispatcher(Framework framework)
        {
            this.framework = framework;
        }

        public void OnEvent(Event e)
        {
            switch (e.TypeId)
            {
                case EventType.OnFrameworkCleared:
                    if (FrameworkCleared != null)
                        FrameworkCleared(this, new FrameworkEventArgs(((OnFrameworkCleared)e).Framework));
                    break;
            }
        }
    }
}