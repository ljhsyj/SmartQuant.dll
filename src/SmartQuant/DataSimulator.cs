// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.Threading;
using System.Collections.Generic;

namespace SmartQuant
{
    class DataSimulator : Provider, IDataSimulator
    {
        private Thread thread;
        private volatile bool running;
        private volatile bool exit;

        private long long_0;

        private LinkedList<Class25> linkedList_0;

        public DateTime DateTime1 { get; set; }

        public DateTime DateTime2 { get; set; }

        public bool SubscribeBid { get; set; }

        public bool SubscribeAsk { get; set; }

        public bool SubscribeTrade { get; set; }

        public bool SubscribeQuote { get; set; }

        public bool SubscribeBar { get; set; }

        public bool SubscribeLevelII { get; set; }

        public bool SubscribeNews { get; set; }

        public bool SubscribeFundamental { get; set; }

        public bool SubscribeAll
        {
            set
            {
                SubscribeBid = value;
                SubscribeAsk = value;
                SubscribeTrade = value;
                SubscribeQuote = value;
                SubscribeBar = value;
                SubscribeLevelII = value;
                SubscribeNews = value;
                SubscribeFundamental = value;
            }
        }

        public BarFilter BarFilter { get; private set; }

        public DataProcessor Processor { get; set; }

        public List<IDataSeries> Series { get; set; }

        public DataSimulator(Framework framework)
            : base(framework)
        {
            this.id = ProviderId.DataSimulator;
            this.name = "DataSimulator";
            this.description = "Default data simulator";
            this.url = "www.smartquant.com";
            DateTime1 = DateTime.MinValue;
            DateTime2 = DateTime.MaxValue;
            SubscribeAll = true;

            this.linkedList_0 = new LinkedList<Class25>();
            Processor = new DataProcessor();
            BarFilter = new BarFilter();
            Series = new List<IDataSeries>();
        }

        public override void Connect()
        {
            if (IsConnected)
                return;
            Status = ProviderStatus.Connected;
        }

        public override void Disconnect()
        {
            if (IsDisconnected)
                return;
            this.exit = true;
            while (this.running)
                Thread.Sleep(1);
            Clear();
            Status = ProviderStatus.Disconnected;
        }

        protected override void OnConnected()
        {
            foreach (var s in Series)
                OpenDataQueue(s, DateTime1, DateTime2);
        }

        protected override void OnDisconnected()
        {
            // no-op
        }

        public void Clear()
        {
            DateTime1 = DateTime.MinValue;
            DateTime2 = DateTime.MaxValue;
            Series.Clear();
            BarFilter.Clear();
        }

        public override void Subscribe(Instrument instrument)
        {
            if (!this.running)
            {
                Subscribe(instrument, DateTime1, DateTime2);
                Start();
            }
            else
                Subscribe(instrument, this.framework.Clock.DateTime, DateTime2);
        }

        public override void Subscribe(InstrumentList instruments)
        {
            if (!this.running)
            {
                foreach (var instrument in instruments)
                    Subscribe(instrument, DateTime1, DateTime2);
                Start();
            }
            else
            {
                foreach (var instrument in instruments)
                    Subscribe(instrument, this.framework.Clock.DateTime, DateTime2);
            }
        }

        // core method
        private void Subscribe(Instrument instrument, DateTime dateTime1, DateTime dateTime2)
        {
            Console.WriteLine("{0} DataSimulator::Subscribe {1}", DateTime.Now, instrument.Symbol);
            if (SubscribeTrade)
            {
                var ds =  this.framework.DataManager.GetDataSeries(instrument, DataObjectType.Trade, BarType.Time, 60);
                OpenDataQueue(ds, dateTime1, dateTime2);
            }
            if (SubscribeBid)
            {
                var ds =  this.framework.DataManager.GetDataSeries(instrument, DataObjectType.Bid, BarType.Time, 60);
                OpenDataQueue(ds, dateTime1, dateTime2);
            }
            if (SubscribeAsk)
            {
                var ds =  this.framework.DataManager.GetDataSeries(instrument, DataObjectType.Ask, BarType.Time, 60);
                OpenDataQueue(ds, dateTime1, dateTime2);
            }
            if (SubscribeQuote)
            {
                var ds =  this.framework.DataManager.GetDataSeries(instrument, DataObjectType.Quote, BarType.Time, 60);
                OpenDataQueue(ds, dateTime1, dateTime2);
            }
            if (SubscribeBar)
            {
                foreach (var ds in this.framework.DataManager.GetDataSeriesList(instrument, "Bar"))
                    if (!IsFilteredOut(ds))
                        OpenDataQueue(ds, dateTime1, dateTime2);
            }
            if (SubscribeLevelII)
            {
                var ds = this.framework.DataManager.GetDataSeries(instrument, DataObjectType.Level2, BarType.Time, 60L);
                OpenDataQueue(ds, dateTime1, dateTime2);
            }
            if (SubscribeFundamental)
            {
                var ds = this.framework.DataManager.GetDataSeries(instrument, DataObjectType.Fundamental, BarType.Time, 60L);
                OpenDataQueue(ds, dateTime1, dateTime2);
            }
            if (SubscribeNews)
            {
                var ds = this.framework.DataManager.GetDataSeries(instrument, DataObjectType.News, BarType.Time, 60L);
                OpenDataQueue(ds, dateTime1, dateTime2);
            }
        }

        private bool IsFilteredOut(DataSeries dataSeries)
        {
            if (BarFilter.Count == 0)
                return false;
            BarType barType;
            long barSize;
            DataSeriesNameHelper.TryGetBarTypeSize(dataSeries, out barType, out barSize);
            return BarFilter.Contains(barType, barSize);
        }

        private void Start()
        {
            this.thread = new Thread(new ThreadStart(Run));
            this.thread.Name = "Data Simulator Thread";
            this.thread.IsBackground = true;
            this.thread.Start();
        }

        private void Run()
        {
            Console.WriteLine("{0} Data simulator thread started", DateTime.Now);
            if (!IsConnected)
                Connect();
            var queue = new EventQueue(EventQueueId.Data, EventQueueType.Master, EventQueuePriority.Normal, 16);
            queue.IsSynched = true;
            queue.Enqueue(new Event[] { new OnQueueOpened(queue), new OnSimulatorStart(DateTime1, DateTime2, 0), new OnQueueClosed(queue) });
            this.framework.EventBus.DataPipe.Add(queue);
            this.running = true;
            this.exit = false;
            while (!this.exit)
            {
                LinkedListNode<Class25> linkedListNode1 = this.linkedList_0.First;
                LinkedListNode<Class25> linkedListNode2 =   null;
                for (; linkedListNode1 != null; linkedListNode1 = linkedListNode1.Next)
                {
                    Class25 class25 = linkedListNode1.Data;
                    if (!class25.done)
                    {
                        if (class25.method_1())
                            ++this.long_0;
                        //Console.WriteLine("long:{0}",this.long_0);
                        linkedListNode2 = linkedListNode1;
                    }
                    else
                    {
                        if (linkedListNode2 == null)
                            this.linkedList_0.First = linkedListNode1.Next;
                        else
                            linkedListNode2.Next = linkedListNode1.Next;
                        --this.linkedList_0.Count;
                        class25.eventQueue_0.Enqueue(new OnQueueClosed(class25.eventQueue_0));
                    }
                }
            }   
            this.exit = false;
            this.running = false;
            Console.WriteLine("{0} Data simulator thread stopped", DateTime.Now);
        }

        private void OpenDataQueue(IDataSeries dataSeries, DateTime dateTime1, DateTime dateTime2)
        {
            if (dataSeries != null)
            {
                var q = new EventQueue(EventQueueId.Data, EventQueueType.Master, EventQueuePriority.Normal, 32768);
                q.IsSynched = true;
                q.Name = dataSeries.Name;
                q.Enqueue(new OnQueueOpened(q));
                this.framework.EventBus.DataPipe.Add(q);
                this.linkedList_0.Add(new Class25(dataSeries, dateTime1, dateTime2, q, Processor));
            }
        }
    }

    class Class25
    {
        internal IDataSeries dataSeries;
        internal EventQueue eventQueue_0;
        internal EventQueue eventQueue_1;
        internal long index1;
        internal long index2;
        internal long current;
        internal long long_3;
        internal DataObject dataObject_0;
        internal bool done;
        internal int stepSize;
        internal long stepCount;
        internal int percent;
        internal DataProcessor processor;

        internal Class25(IDataSeries dataSeries, DateTime dateTime1, DateTime dateTime2, EventQueue eventQueue_2, DataProcessor processor)
        {
            this.eventQueue_1 = new EventQueue(EventQueueId.All, EventQueueType.Master, EventQueuePriority.Normal, 128);
            this.dataSeries = dataSeries;
            this.eventQueue_0 = eventQueue_2;
            this.processor =  processor ?? new DataProcessor();
            this.index1 = dateTime1 == DateTime.MinValue || dateTime1 < dataSeries.DateTime1 ? 0 : dataSeries.GetIndex(dateTime1, SearchOption.Next);
            this.index2 = dateTime2 == DateTime.MaxValue || dateTime2 > dataSeries.DateTime2 ? dataSeries.Count - 1 : dataSeries.GetIndex(dateTime2, SearchOption.Prev);
            this.current = this.index1;
            this.stepSize = (int) Math.Ceiling(this.index2 - this.index1 + 1 / 100d);
            this.stepCount = this.stepSize;
            this.percent = 0;
        }

        internal bool method_1()
        {
            if (this.eventQueue_0.IsFull())
                return false;
            DataObject dataObject;
            while (this.eventQueue_1.IsEmpty())
            {
                if (this.dataObject_0 == null)
                {
                    if (this.current <= this.index2)
                    {
                        this.dataObject_0 = this.dataSeries[this.current];
                        this.dataObject_0 = this.processor.Process(this);
                        ++this.current;
                        ++this.long_3;
                    }
                    else
                    {
                        this.done = true;
                        return false;
                    }
                }
                else
                {
                    dataObject = this.dataObject_0;
                    this.dataObject_0 = null;
                    goto label_8;
                }
            }
            dataObject = (DataObject)this.eventQueue_1.Read();
            label_8:
            this.eventQueue_0.Write(dataObject);
            if (this.long_3 ==  this.stepCount)
            {
                this.stepCount += this.stepSize;
                ++this.percent;
                this.eventQueue_0.Enqueue(new OnSimulatorProgress(this.stepCount, this.percent));
            }
            return true;
        }
    }
}
