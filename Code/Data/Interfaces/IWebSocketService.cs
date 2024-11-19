using FabSandbox.Data.Models;

namespace FabSandbox.Data.Interfaces
{
    public interface IWebSocketService
    {
        bool IsConnected { get; }

        event Action OnConnectionStateChanged;
        event Action<DeviceMessage> OnLdrDataReceived;
        event Action<DeviceMessage> OnLedDataReceived;
        event Action<DeviceMessage> OnPotentiometerDataReceived;

        Task ConnectAsync(string uri);
        Task DisconnectAsync();
        Task SendAsync(string message);
    }
}