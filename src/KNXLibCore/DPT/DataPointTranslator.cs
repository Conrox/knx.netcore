using System;
using System.Collections.Generic;
using System.Linq;

namespace KNXLib.DPT
{
    public sealed class DataPointTranslator
    {
        public static readonly DataPointTranslator Instance = new DataPointTranslator();
        private readonly IDictionary<string, DataPoint> _dataPoints = new Dictionary<string, DataPoint>();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static DataPointTranslator()
        {
        }

        private DataPointTranslator()
        {
            Type type = typeof(DataPoint);
            //IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
            //                .SelectMany(s => s.GetTypes())
            //                .Where(p => type.IsAssignableFrom(p) && p != type);

            IEnumerable<Type> types = new List<Type>() {
                typeof(DataPoint1Bit),
                typeof(DataPoint2ByteFloatTemperature),
                typeof(DataPoint3BitControl),
                typeof(DataPoint8BitNoSignNonScaledValue1UCount),
                typeof(DataPoint8BitNoSignScaledAngle),
                typeof(DataPoint8BitNoSignScaledPercentU8),
                typeof(DataPoint8BitNoSignScaledScaling),
                typeof(DataPoint8BitSignRelativeValue),
            };

            foreach (Type t in types)
            {
                DataPoint dp = (DataPoint)Activator.CreateInstance(t);

                foreach (string id in dp.Ids)
                {
                    _dataPoints.Add(id, dp);
                }
            }
        }

        public object FromDataPoint(string type, string data)
        {
            try
            {
                DataPoint dpt;
                if (_dataPoints.TryGetValue(type, out dpt))
                    return dpt.FromDataPoint(data);
            }
            catch (Exception)
            {
            }

            return null;
        }

        public object FromDataPoint(string type, byte[] data)
        {
            try
            {
                DataPoint dpt;
                if (_dataPoints.TryGetValue(type, out dpt))
                    return dpt.FromDataPoint(data);
            }
            catch
            {
            }

            return null;
        }

        public byte[] ToDataPoint(string type, string value)
        {
            try
            {
                DataPoint dpt;
                if (_dataPoints.TryGetValue(type, out dpt))
                    return dpt.ToDataPoint(value);
            }
            catch
            {
            }

            return null;
        }

        public byte[] ToDataPoint(string type, object value)
        {
            try
            {
                DataPoint dpt;
                if (_dataPoints.TryGetValue(type, out dpt))
                    return dpt.ToDataPoint(value);
            }
            catch
            {
            }

            return null;
        }
    }
}
