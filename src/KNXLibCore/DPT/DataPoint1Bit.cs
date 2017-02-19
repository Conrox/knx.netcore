using System;
using System.Globalization;
using KNXLib.Log;

namespace KNXLib.DPT
{
    internal sealed class DataPoint1Bit : DataPoint
    {
        public override string[] Ids
        {
            get
            {                
                return new[] { "1.001", "1.002", "1.003", "1.004", "1.005", "1.006" /*to 22*/};
            }
        }

        public override object FromDataPoint(byte[] data)
        {
            throw new NotImplementedException();
        }

        public override object FromDataPoint(string data)
        {
            if (data == "true" || data == "\u0001")
                return true;
            else if (data == "false" || data == "\0")
                return false;
            else
                return false;
        }

        public override byte[] ToDataPoint(object value)
        {
            throw new NotImplementedException();
        }

        public override byte[] ToDataPoint(string value)
        {
            throw new NotImplementedException();
        }
    }
}
