using FabSandbox.Data.Models;

namespace FabSandbox.Data.Interfaces
{
    public interface IDeviceServices
    {
        bool IsConnected { get; }
        event Action? OnStateChanged;
        event Action<DeviceMessage>? OnDataReceived;
        DeviceModel DeviceModel { get; }
        string DeviceName { get; }
        string DeviceKey { get; }

        Task GetConfigData();
        int GetGPIOValue();
        int GetNotifyValue();
        void SetDeviceModel(string deviceName, string deviceKey);
        Task SendDeviceMessageAsync(string messageType, object? value = null);
        void SetGPIOValue(int gpio);
        Task SetGPIOValueShowDialog();
        void SetNotifyIntervalValue(int interval);
        Task SetNotifyIntervalValueShowDialog();
        Task SetValueDialog(string messageType, string title, string units, int value);
        void ShowMessage(string messageText);
    }
}