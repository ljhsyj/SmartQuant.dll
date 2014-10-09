using System;

namespace SmartQuant
{
    public class EventDispatcher
    {
        protected internal Framework framework;

        public event FrameworkEventHandler FrameworkCleared;

        public event InstrumentEventHandler InstrumentAdded;

        public event InstrumentEventHandler InstrumentDeleted;

        public event InstrumentDefinitionEventHandler InstrumentDefinition;

        public event InstrumentDefinitionEndEventHandler InstrumentDefinitionEnd;

        public event ProviderEventHandler ProviderAdded;

        public event ProviderEventHandler ProviderRemoved;

        public event ProviderEventHandler ProviderStatusChanged;

        public event ProviderEventHandler ProviderConnected;

        public event ProviderEventHandler ProviderDisconnected;

        public event ExecutionCommandEventHandler ExecutionCommand;

        public event ExecutionReportEventHandler ExecutionReport;

        public event OrderManagerClearedEventHandler OrderManagerCleared;

        public event FillEventHandler Fill;

        public event TransactionEventHandler Transaction;

        public event BarEventHandler Bar;

        public event BidEventHandler Bid;

        public event AskEventHandler Ask;

        public event TradeEventHandler Trade;

        public event ProviderErrorEventHandler ProviderError;

        public event HistoricalDataEventHandler HistoricalData;

        public event HistoricalDataEndEventHandler HistoricalDataEnd;

        public event PortfolioEventHandler PortfolioAdded;

        public event PortfolioEventHandler PortfolioDeleted;

        public event PositionEventHandler PositionOpened;

        public event PositionEventHandler PositionChanged;

        public event PositionEventHandler PositionClosed;

        public event PortfolioEventHandler PortfolioParentChanged;

        public event GroupEventHandler NewGroup;

        public event GroupEventEventHandler NewGroupEvent;

        public event GroupUpdateEventHandler NewGroupUpdate;

        public event SimulatorProgressEventHandler SimulatorProgress;

        public event EventHandler SimulatorStop;

        public event AccountDataEventHandler AccountData;

        public event EventHandler EventManagerStarted;

        public event EventHandler EventManagerStopped;

        public event EventHandler EventManagerPaused;

        public event EventHandler EventManagerResumed;

        public event EventHandler EventManagerStep;

        public EventDispatcher(Framework framework)
        {
            this.framework = framework;
        }

        public void OnEvent(Event e)
        {
            switch (e.TypeId)
            {
                case EventType.Bid:
                    if (Bid != null)
                        Bid(this, (Bid)e);
                    break;
                case EventType.Ask:
                    if (Ask != null)
                        Ask(this, (Ask)e);
                    break;
                case EventType.Trade:
                    if (Trade != null)
                        Trade(this, (Trade)e);
                    break;
                case EventType.Bar:
                    if (Bar != null)
                        Bar(this, (Bar)e);
                    break;
                case EventType.ExecutionReport:
                    if (ExecutionReport != null)
                        ExecutionReport(this, (ExecutionReport)e);
                    break;
                case EventType.ExecutionCommand:
                    if (ExecutionCommand != null)
                        ExecutionCommand(this, (ExecutionCommand)e);
                    break;
                case EventType.ProviderError:
                    if (ProviderError != null)
                        ProviderError(this, new ProviderErrorEventArgs((ProviderError)e));
                    break;
                case EventType.Group:
                    if (NewGroup != null)
                        NewGroup(this, new GroupEventAgrs((Group)e));
                    break;
                case EventType.GroupUpdate:
                    if (NewGroupUpdate != null)
                        NewGroupUpdate(this, new GroupUpdateEventAgrs((GroupUpdate)e));
                    break;
                case EventType.GroupEvent:
                    if (NewGroupEvent != null)
                        NewGroupEvent(this, new GroupEventEventAgrs((GroupEvent)e));
                    break;
                case EventType.OnFrameworkCleared:
                    if (FrameworkCleared != null)
                        FrameworkCleared(this, new FrameworkEventArgs(((OnFrameworkCleared)e).Framework));
                    break;
                case EventType.OnInstrumentAdded:
                    if (InstrumentAdded != null)
                        InstrumentAdded(this, new InstrumentEventArgs(((OnInstrumentAdded)e).Instrument));
                    break;
                case EventType.OnInstrumentDeleted:
                    if (InstrumentDeleted != null)
                        InstrumentDeleted(this, new InstrumentEventArgs(((OnInstrumentDeleted)e).Instrument));
                    break;
                case EventType.OnProviderAdded:
                    if (ProviderAdded != null)
                        ProviderAdded(this, new ProviderEventArgs(((OnProviderAdded)e).Provider));
                    break;
                case EventType.OnProviderRemoved:
                    if (ProviderRemoved != null)
                        ProviderRemoved(this, new ProviderEventArgs(((OnProviderRemoved)e).Provider));
                    break;
                case EventType.OnProviderConnected:
                    if (ProviderConnected != null)
                        ProviderConnected(this, new ProviderEventArgs(((OnProviderConnected)e).Provider));
                    break;
                case EventType.OnProviderDisconnected:
                    if (ProviderDisconnected != null)
                        ProviderDisconnected(this, new ProviderEventArgs(((OnProviderDisconnected)e).Provider));
                    break;
                case EventType.OnProviderStatusChanged:
                    if (ProviderStatusChanged != null)
                        ProviderStatusChanged(this, new ProviderEventArgs(((OnProviderStatusChanged)e).Provider));
                    break;
                case EventType.OnSimulatorStop:
                    if (SimulatorStop != null)
                        SimulatorStop(this, EventArgs.Empty);
                    break;
                case EventType.OnSimulatorProgress:
                    if (SimulatorProgress != null)
                        SimulatorProgress(this, new SimulatorProgressEventArgs(((OnSimulatorProgress)e).Count, ((OnSimulatorProgress)e).Percent));
                    break;
                case EventType.OnPositionOpened:
                    if (PositionOpened != null)
                        PositionOpened(this, new PositionEventArgs(((OnPositionOpened)e).Portfolio, ((OnPositionOpened)e).Position));
                    break;
                case EventType.OnPositionClosed:
                    if (PositionClosed != null)
                        PositionClosed(this, new PositionEventArgs(((OnPositionClosed)e).Portfolio, ((OnPositionClosed)e).Position));
                    break;
                case EventType.OnPositionChanged:
                    if (PositionChanged != null)
                        PositionChanged(this, new PositionEventArgs(((OnPositionChanged)e).Portfolio, ((OnPositionChanged)e).Position));
                    break;
                case EventType.OnFill:
                    if (Fill != null)
                        Fill(this, (OnFill)e);
                    break;
                case EventType.OnTransaction:
                    if (Transaction != null)
                        Transaction(this, (OnTransaction)e);
                    break;
                case EventType.OnOrderManagerCleared:
                    if (OrderManagerCleared != null)
                        OrderManagerCleared(this, (OnOrderManagerCleared)e);
                    break;
                case EventType.OnInstrumentDefinition:
                    if (InstrumentDefinition != null)
                        InstrumentDefinition(this, new InstrumentDefinitionEventArgs(((OnInstrumentDefinition)e).Definition));
                    break;
                case EventType.OnInstrumentDefintionEnd:
                    if (InstrumentDefinitionEnd != null)
                        InstrumentDefinitionEnd(this, new InstrumentDefinitionEndEventArgs(((OnInstrumentDefinitionEnd)e).End));
                    break;
                case EventType.OnPortfolioAdded:
                    if (PortfolioAdded != null)
                        PortfolioAdded(this, new PortfolioEventArgs(((OnPortfolioAdded)e).Portfolio));
                    break;
                case EventType.OnPortfolioDeleted:
                    if (PortfolioDeleted != null)
                        PortfolioDeleted(this, new PortfolioEventArgs(((OnPortfolioDeleted)e).Portfolio));
                    break;
                case EventType.OnPortfolioParentChanged:
                    if (PortfolioParentChanged != null)
                        PortfolioParentChanged(this, new PortfolioEventArgs(((OnPortfolioParentChanged)e).Portfolio));
                    break;
                case EventType.HistoricalData:
                    if (HistoricalData != null)
                        HistoricalData(this, new HistoricalDataEventArgs((HistoricalData)e));
                    break;
                case EventType.HistoricalDataEnd:
                    if (HistoricalDataEnd != null)
                        HistoricalDataEnd(this, new HistoricalDataEndEventArgs((HistoricalDataEnd)e));
                    break;
                case EventType.AccountData:
                    if (AccountData != null)
                        AccountData(this, new AccountDataEventArgs((AccountData)e));
                    break;
                case EventType.OnEventManagerStarted:
                    if (EventManagerStarted != null)
                        EventManagerStarted(this, EventArgs.Empty);
                    break;
                case EventType.OnEventManagerStopped:
                    if (EventManagerStopped != null)
                        EventManagerStopped(this, EventArgs.Empty);
                    break;
                case EventType.OnEventManagerPaused:
                    if (EventManagerPaused != null)
                        EventManagerPaused(this, EventArgs.Empty);
                    break;
                case EventType.OnEventManagerResumed:
                    if (EventManagerResumed != null)
                        EventManagerResumed(this, EventArgs.Empty);
                    break;
                case EventType.OnEventManagerStep:
                    if (EventManagerStep != null)
                        EventManagerStep(this, EventArgs.Empty);
                    break;
            }
        }
    }
}