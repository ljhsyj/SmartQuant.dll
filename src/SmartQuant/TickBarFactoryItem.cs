namespace SmartQuant
{
    public class TickBarFactoryItem : BarFactoryItem
    {
        public TickBarFactoryItem(Instrument instrument, long barSize, BarInput barInput = BarInput.Trade)
            : base(instrument, BarType.Tick, barSize, barInput)
        {
        }

        protected internal override void OnData(DataObject obj)
        {
            base.OnData(obj);
            if (this.bar.N == this.barSize)
                EmitBar();
        }
    }
}
