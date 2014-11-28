// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading;

namespace SmartQuant
{
    class ReminderEventQueue : SortedEventQueue
    {
        public ReminderEventQueue()
            : base(EventQueueId.Reminder)
        {
        }

        public void Remove(ReminderCallback callback, DateTime dateTime)
        {
            lock (this)
            {
                for (int i = 0; i < this.events.Count; ++i)
                {
                    var reminder = (Reminder)this.events[i];
                    if (reminder.Callback == callback && reminder.DateTime == dateTime)
                    {
                        this.events.Pop();
                        if (i != 0 || this.events.Count == 0)
                            break;
                        this.dateTime = this.events[0].DateTime;
                        break;
                    }
                }
            }
        }
    }

    public class Clock
    {
        private Framework framework;
        private ClockType type;
        private bool isStandalone;
        private ClockMode mode;
        private DateTime dateTime;
        private long ticks;

        internal IEventQueue Queue { get; set; }

        public DateTime DateTime
        {
            get
            {
                return this.type == ClockType.Exchange || Mode == ClockMode.Simulation ? this.dateTime : DateTime.Now;
            }
            internal set
            {
                if (Mode == ClockMode.Realtime)
                {
                    Console.WriteLine("Clock::DateTime Can not set dateTime because Clock is not in the Simulation mode");
                    return;
                }

                if (this.type == ClockType.Exchange)
                {
                    if (this.dateTime > value)
                        Console.WriteLine("Clock::DateTime (Exchange) incorrect set order");
                    else
                        this.dateTime = value;
                }

                // throw new NotImplementedException();
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
                if (this.mode == value)
                    return;
                this.mode = value;
                if (this.mode == ClockMode.Simulation)
                    Clear();
            }
        }

        public ClockResolution Resolution { get; set; }

        public long Ticks
        {
            get
            { 
                return DateTime.Ticks;
//                return Mode == ClockMode.Simulation ? this.dateTime.Ticks : DateTime.Now.Ticks;
            }
        }

        public Clock(Framework framework, ClockType type = ClockType.Local, ClockMode mode = ClockMode.Simulation, bool isStandalone = false)
        {
            this.framework = framework;
            this.type = type;
            this.dateTime = DateTime.MinValue;
            this.ticks = DateTime.Now.Ticks;
            this.isStandalone = isStandalone;
            Mode = mode;
            Queue = new ReminderEventQueue();
//            if (isStandalone)
//            {
//                Thread thread = new Thread(new ThreadStart(Run));
//                thread.Name = "Clock Thread";
//                thread.IsBackground = true;
//                thread.Start();
//            }
        }

        public bool AddReminder(ReminderCallback callback, DateTime dateTime, object data = null)
        {
            return AddReminder(new Reminder(callback, dateTime, data) { Clock = this });
        }

        public bool AddReminder(Reminder reminder)
        {
            if (reminder.DateTime < DateTime)
            {
                Console.WriteLine("Clock::AddReminder ({0}) Can not set reminder to the past. Clock datetime = {1} Reminder datetime = {2} Reminder object = {3}", this.type, DateTime.ToString("dd.MM.yyyy HH:mm:ss.ffff"), reminder.DateTime.ToString("dd.MM.yyyy HH:mm:ss.ffff"), reminder.Data);
                return false;
            }
            reminder.Clock = this;
            Queue.Enqueue(reminder);
            return true;
        }

        public void RemoveReminder(ReminderCallback callback, DateTime dateTime)
        {
            ((ReminderEventQueue)Queue).Remove(callback, dateTime);
        }

        public void Clear()
        {
            this.dateTime = DateTime.MinValue;
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

        //        private void Run()
        //        {
        //            Console.WriteLine("{0} Clock thread started", DateTime.Now);
        //            bool pending = false;
        //            while (true)
        //            {
        //                while (Mode != ClockMode.Realtime)
        //                    Thread.Sleep(10);
        //                if (!Queue.IsEmpty())
        //                {
        //                    var ticks1 = this.Queue.PeekDateTime().Ticks;
        //                    var ticks2 = this.framework.Clock.Ticks;
        //                    if (ticks1 <= ticks2)
        //                        ((Reminder)Queue.Read()).Execute();
        //                    else if (ticks1 - ticks2 < 15000)
        //                        pending = true;
        //                }
        //                if (pending)
        //                    Thread.SpinWait(1);
        //                else
        //                    Thread.Sleep(1);
        //            }
        //        }
    }
}
