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
        private ClockMode mode;
        private DateTime dateTime;
        private long ticks;

        public DateTime DateTime
        {
            get
            {
//                if (this.type == ClockType.Exchange || this.mode == ClockMode.Simulation)
//                    return this.dateTime;
//                if (Resolution == ClockResolution.Normal)
//                    return DateTime.Now;
//                else
//                    return new DateTime(this.ticks + (long) ((double) this.stopwatch_0.ElapsedTicks / (double) Stopwatch.Frequency * 10000000.0));
//
                return this.dateTime;
            }
            private set
            {
            }
        }

        public ClockMode Mode
        { 
            get
            {
                return this.mode;
            } 
            set
            {
                if (this.mode != value)
                {
                    this.mode = value;
                    if (this.mode == ClockMode.Simulation)
                        this.dateTime = DateTime.MinValue;
                }
            }
        }

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
