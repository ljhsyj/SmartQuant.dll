//
// Author: Alex Lee <lu.lee05@gmail.com>
//

using System;

namespace SmartQuant
{
    public class OnSimulatorStop : DataObject
    {
        public override byte TypeId
        {
            get
            {
                return DataObjectType.OnSimulatorStop;
            }
        }

        public OnSimulatorStop()
        {
            DateTime = DateTime.MinValue;
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
