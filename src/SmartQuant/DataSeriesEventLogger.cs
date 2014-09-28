using System;

namespace SmartQuant
{
    public class DataSeriesEventLogger : EventLogger
    {
        private DataSeries series;

        public DataSeriesEventLogger(Framework framework, DataSeries series)
            : base(framework, "DataSeriesEventLogger")
        {
            this.series = series;
        }

        public DataSeriesEventLogger(DataSeries series)
            : base(Framework.Current, "DataSeriesEventLogger")
        {
            this.series = series;
        }

        public void Enable(byte typeId)
        {
        }

        public void Disable(byte typeId)
        {
        }

        public override void OnEvent(Event e)
        {
        }
    }
}

