using System;

namespace SmartQuant
{
    public class News : DataObject
    {
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
        {
        }
    }
}

