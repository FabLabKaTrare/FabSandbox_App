using System;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FabSandbox.Data.Config;
using FabSandbox.Data.Models;
using FabSandbox.Data.Interfaces;

namespace FabSandbox.Data.Services
{

    public class WebSocketService : IWebSocketService
    {
        private ClientWebSocket _webSocket;
        public bool IsConnected => _webSocket?.State == WebSocketState.Open;

        public event Action OnConnectionStateChanged;
        public event Action<DeviceMessage> OnPotentiometerDataReceived;
        public event Action<DeviceMessage> OnLdrDataReceived;
        public event Action<DeviceMessage> OnLedDataReceived;

        public async Task ConnectAsync(string uri)
        {
            try
            {
                _webSocket = new ClientWebSocket();
                Uri siteUri = new($"ws://{uri}/ws");
                await _webSocket.ConnectAsync(siteUri, CancellationToken.None);
                OnConnectionStateChanged?.Invoke();
                await ReceiveMessages();
            }
            catch (Exception)
            {
                OnConnectionStateChanged?.Invoke();
                throw;
            }

        }

        public async Task DisconnectAsync()
        {
            if (_webSocket != null)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnecting", CancellationToken.None);
                _webSocket = null;
                OnConnectionStateChanged?.Invoke();
            }
        }

        private async Task ReceiveMessages()
        {
            var buffer = new byte[1024 * 4];
            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await DisconnectAsync();
                }
                else
                {
                    var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    HandleMessage(json);
                }
            }
        }

        private void HandleMessage(string json)
        {
            try
            {
                var message = JsonSerializer.Deserialize<DeviceMessage>(json);

                if (message != null)
                {

                    OnLedDataReceived?.Invoke(message);
                    //switch (message.Device)
                    //{
                    //    case SystemConstants.DeviceKey.Potentiometer:
                    //        OnPotentiometerDataReceived?.Invoke(message);
                    //        break;
                    //    case SystemConstants.DeviceKey.LDR:
                    //        OnLdrDataReceived?.Invoke(message);
                    //        break;
                    //    case SystemConstants.DeviceKey.LED:
                    //        OnLedDataReceived?.Invoke(message);
                    //        break;
                    //}
                }
            }
            catch (JsonException)
            {
                // Handle JSON deserialization error
            }
        }

        public async Task SendAsync(string message)
        {
            try
            {
                if (_webSocket != null && _webSocket.State == WebSocketState.Open)
                {
                    var buffer = Encoding.UTF8.GetBytes(message);

                    await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    throw new ArgumentException("Websocket: la conexión no se ha establecido");
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

    }

}
