// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace SmartQuant
{
    public class Portfolio
    {
        internal Framework framework;

        private Portfolio parent;

        internal int Id { get; set; }

        public string Name { get; private set; }

        [Browsable(false)]
        public Account Account { get; private set; }

        public List<Portfolio> Children { get; private set; }

        [Browsable(false)]
        public Portfolio Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                if (this.parent == value)
                    return;
                if (this.parent != null)
                    this.parent.Children.Remove(this);
                this.parent = value;
                if (this.parent != null)
                {
                    Account.Parent = this.parent.Account;
                    this.parent.Children.Add(this);
                }
                else
                    Account.Parent = null;
                this.framework.EventServer.OnPortfolioParentChanged(this, true);
            }
        }

        public Pricer Pricer { get; set; }

        [Browsable(false)]
        public List<Transaction> Transactions { get; private set; }

        internal IdArray<Transaction> TransactionsByOrderId { get; private set; }

        [Browsable(false)]
        public PortfolioPerformance Performance { get; private set; }

        [Browsable(false)]
        public List<Position> Positions { get; private set; }

        internal IdArray<Position> PositionsByInstrumentId { get; private set; }

        public double Value
        {
            get
            {
                return AccountValue + PositionValue;
            }
        }

        public double AccountValue
        {
            get
            {
                return Account.Value;
            }
        }

        public double PositionValue
        {
            get
            {
                double num = 0;
//                for (int index = 0; index < this.list_1.Count; ++index)
//                    num += this.framework_0.ginterface4_0.Convert(this.list_1[index].Value, this.list_1[index].instrument_0.byte_0, this.account_0.byte_0);
                return num;
            }
        }

        [Browsable(false)]
        public FillSeries Fills { get; private set; }

        [Browsable(false)]
        public PortfolioStatistics Statistics { get; private set; }

        public Portfolio(Framework framework, string name = "")
        {
            this.framework = framework;
            Name = name;
            Children = new List<Portfolio>();
            Transactions = new List<Transaction>();
            TransactionsByOrderId = new IdArray<Transaction>(131072);
            PositionsByInstrumentId = new IdArray<Position>(8192);
            Positions = new List<Position>();
            Account = new Account(framework);
            Fills = new FillSeries(name);
            Pricer = framework.PortfolioManager.Pricer;
            Performance = new PortfolioPerformance(this);
            Statistics = new PortfolioStatistics(this);
        }

        public void Add(Fill fill)
        {
            throw new NotImplementedException();
        }

        internal Position Add(Instrument instrument)
        {
            var position = PositionsByInstrumentId[instrument.Id];
            if (position == null)
            {
                position = new Position(this, instrument);
                PositionsByInstrumentId[instrument.Id] = position;
                Positions.Add(position);
            }
            return position;
        }

        public bool HasPosition(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public bool HasPosition(Instrument instrument, PositionSide side, double qty)
        {
            throw new NotImplementedException();
        }

        public Position GetPosition(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public bool HasLongPosition(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public bool HasLongPosition(Instrument instrument, double qty)
        {
            throw new NotImplementedException();
        }

        public bool HasShortPosition(Instrument instrument)
        {
            throw new NotImplementedException();
        }

        public bool HasShortPosition(Instrument instrument, double qty)
        {
            throw new NotImplementedException();
        }

        internal void OnExecutionReport(ExecutionReport report)
        {
            OnExecutionReport(report, false);
        }

        internal void OnExecutionReport(ExecutionReport report, bool queued = true)
        {
            Transaction transaction;
            switch (report.ExecType)
            {
                case ExecType.ExecRejected:
                case ExecType.ExecCancelled:
                    transaction = TransactionsByOrderId[report.Order.Id];
                    if (transaction == null)
                        break;
                    transaction.IsDone = true;
                    OnTransaction(transaction, true);
                    break;
                case ExecType.ExecTrade:
                    transaction = TransactionsByOrderId[report.Order.Id];
                    if (transaction == null)
                    {
                        transaction = new Transaction();
                        method_4(transaction, true);
                        TransactionsByOrderId[report.Order.Id] = transaction;
                    }
                    Fill fill = new Fill(report);
                    transaction.Add(fill);
                    OnFill(fill, queued);
                    if (report.OrdStatus == OrderStatus.Filled)
                    {
                        transaction.IsDone = true;
                        OnTransaction(transaction, true);
                    }
                    break;
            }
        }

        internal void method_4(Transaction transaction, bool queued = true)
        {
            Transactions.Add(transaction);
            if (Parent != null)
                Parent.method_4(transaction, queued);
        }

        internal void OnTransaction(Transaction transaction, bool queued = true)
        {
            this.framework.EventServer.OnTransaction(this, transaction, queued);
            if (Parent != null)
                Parent.OnTransaction(transaction, queued);
            Statistics.OnTransaction(transaction);
        }

        internal void OnFill(Fill fill, bool queued = true)
        {
            Fills.Add(fill);
            this.framework.EventServer.OnFill(this, fill, queued);
            var instrument = fill.Instrument;
            bool flag = false;
            var position = FindPosition(instrument);
            if (position.Qty == 0)
                flag = true;
            position.Add(fill);
            Account.Add(fill, false);
            if (flag)
            {
                Statistics.OnPositionChanged(position);
                this.framework.EventServer.OnPositionChanged(this, position, queued);
                Statistics.OnPositionOpened(position);
                this.framework.EventServer.OnPositionOpened(this, position, queued);
            }
            else
            {
                this.framework.EventServer.OnPositionChanged(this, position, queued);
                if (position.Qty == 0)
                {
                    Statistics.OnPositionClosed(position);
                    this.framework.EventServer.OnPositionClosed(this, position, queued);
                }
            }
            if (Parent != null)
                Parent.OnFill(fill, queued);
            Statistics.OnFill(fill);
        }

        internal Position FindPosition(Instrument instrument)
        {
            Position position = PositionsByInstrumentId[instrument.Id];
            if (position == null)
            {
                position = new Position(this, instrument);
                PositionsByInstrumentId[instrument.Id] = position;
                Positions.Add(position);
            }
            return position;
        }

        #region Extra Helper Methods

        internal string GetName()
        {
            return Name;
        }

        internal int GetId()
        {
            return Id;
        }

        #endregion
    }
}
