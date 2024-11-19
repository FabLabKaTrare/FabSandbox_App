namespace FabSandbox.Data.Models
{
    public class DeviceModel
    {
        public string DeviceName { get; set; }

        public string DeviceKey { get; set; }

        public int GpioCurrentValue { get; set; } = 0;

        public int GpioDefaultValue { get; set; } = 0;

        public int NotifyInvervalValue { get; set; } = 3000;

        public int NotifyInvervalDefaultValue { get; set; } = 3000;
    }
}
