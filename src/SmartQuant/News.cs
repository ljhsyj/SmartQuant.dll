using System;

namespace SmartQuant
{
    public class News : DataObject
    {
        internal int ProviderId { get; set; }

        internal int InstrumentId { get; set; }

        internal byte Urgency { get; set; }

        internal string Url { get; set; }

        internal  string Headline { get; set; }

        internal  string Text { get; set; }

        public override byte TypeId
        {
            get
            {
                return DataObjectType.News;
            }
        }

        public News()
        {
        }

        public News(DateTime dateTime, int providerId, int instrumentId, byte urgency, string url, string headline, string text)
            : base(dateTime)
        {
            ProviderId = providerId;
            InstrumentId = instrumentId;
            Urgency = urgency;
            Url = url;
            Headline = headline;
            Text = text;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Headline, Text);
        }
    }
}