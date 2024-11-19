using FabSandbox.Data.Models;

namespace FabSandbox.Data.Services
{
    public class DeviceDataService
    {
        private List<Devicedata> Data { get; set; } = [];

        private int maxPoints = 15;

        public List<DataPoint> Add(string deviceName, double value)
        {
            List<DataPoint> values = [];

            if (Data.Any(e => e.DeviceName == deviceName))
            {
                values = Data.FirstOrDefault(e => e.DeviceName == deviceName).Data.TakeLast(maxPoints - 1).ToList();
            }

            values.Add(new DataPoint { Time = DateTime.Now, Value = value });

            if (Data.Any(e => e.DeviceName == deviceName))
            {
                Data.FirstOrDefault(e => e.DeviceName == deviceName).Data = values;
            }
            else
            {
                Data.Add(new Devicedata() { DeviceName = deviceName, Data = values });
            }

            return values;
        }

        public List<DataPoint> Get(string deviceName)
        {
            if (Data.Any(e => e.DeviceName == deviceName))
            {
                return Data.FirstOrDefault(e => e.DeviceName == deviceName)?.Data;
            }
            else
            {
                return [];
            }
            
        }

        private class Devicedata
        {
            public string DeviceName { get; set; }

            public List<DataPoint> Data { get; set; }
        }
    }
}
