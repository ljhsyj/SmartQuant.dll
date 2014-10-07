// Licensed under the Apache License, Version 2.0. 
// Copyright (c) Alex Lee. All rights reserved.

using System;

namespace SmartQuant
{
    public class SellSideStrategy : Strategy, IDataProvider, IExecutionProvider
    {
        public bool IsConnected { get; private set; }

        public bool IsDisconnected { get; private set; }

        public new ProviderStatus Status { get; set; }

        public SellSideStrategy(Framework framework, string name)
            : base(framework, name)
        {
            IsConnected = true;
            IsDisconnected = false;
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void EmitBid(Bid bid)
        {
            this.EventManager.OnEvent(bid);
        }

        public void EmitAsk(Ask ask)
        {
            this.EventManager.OnEvent(ask);
        }

        public void EmitExecutionReport(ExecutionReport report)
        {
            this.EventManager.OnEvent(report);
        }

        public void EmitTrade(Trade trade)
        {
            this.EventManager.OnEvent(trade);
        }

        public virtual void Subscribe(Instrument instrument)
        {
            this.OnSubscribe(instrument);
        }

        public virtual void Subscribe(InstrumentList instruments)
        {
            this.OnSubscribe(instruments);
        }

        public virtual void Unsubscribe(Instrument instrument)
        {
            this.OnUnsubscribe(instrument);
        }

        public virtual void Unsubscribe(InstrumentList instruments)
        {
            this.OnUnsubscribe(instruments);
        }

        public virtual void Send(ExecutionCommand command)
        {
            if (command.Type == ExecutionCommandType.Send)
                this.OnSendCommand(command);
            else if (command.Type == ExecutionCommandType.Cancel)
                this.OnCancelCommand(command);
            else if (command.Type == ExecutionCommandType.Replace)
                this.OnReplaceCommand(command);
        }

        protected virtual void OnSubscribe(InstrumentList instruments)
        {
        }

        protected virtual void OnSubscribe(Instrument instrument)
        {
        }

        protected virtual void OnUnsubscribe(InstrumentList instruments)
        {
        }

        protected virtual void OnUnsubscribe(Instrument instrument)
        {
        }
            
        public virtual void OnSendCommand(ExecutionCommand command)
        {
        }

        public virtual void OnCancelCommand(ExecutionCommand command)
        {
        }

        public virtual void OnReplaceCommand(ExecutionCommand command)
        {
        }
    }
}