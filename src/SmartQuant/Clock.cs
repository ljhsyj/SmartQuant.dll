// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading;

namespace SmartQuant
{
    public class Clock
    {
        private Framework framework;
        private ClockType type;
        private bool isStandalone;

        public DateTime DateTime
        {
            get;
            private set;
        }

        public ClockMode Mode { get; set; }

        public ClockResolution Resolution { get; set; }

        public long Ticks { get { throw new System.NotImplementedException(); } }

        public Clock(Framework framework, ClockType type = ClockType.Local, ClockMode mode = ClockMode.Simulation, bool isStandalone = false)
        {
            this.framework = framework;
            this.Mode = mode;
            this.type = type;
            this.isStandalone = isStandalone;
            if (isStandalone)
            {
                Thread thread = new Thread(new ThreadStart(this.Run));
                thread.Name = "Clock Thread";
                thread.IsBackground = true;
                thread.Start();
            }
        }

        public void AddReminder(ReminderCallback callback, DateTime dateTime, object data = null)
        {
            this.AddReminder(new Reminder(callback, dateTime, data));
        }

        public void AddReminder(Reminder reminder)
        {
        }

        public void RemoveReminder(ReminderCallback callback, DateTime dateTime)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public string GetModeAsString()
        {
            switch (this.Mode)
            {
                case ClockMode.Realtime:
                    return "Realtime";
                case ClockMode.Simulation:
                    return "Simulation";
                default:
                    return "Undefined";
            }
        }

        private void Run()
        {
            Console.WriteLine(string.Format("{0} Clock thread started", DateTime.Now));
            while (true)
                Thread.Sleep(1);
        }
    }
}
