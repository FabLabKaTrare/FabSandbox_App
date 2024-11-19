using FabSandbox.Components.Common;
using FabSandbox.Data.Config;
using FabSandbox.Data.Interfaces;
using FabSandbox.Data.Models;
using MudBlazor;
using System.Text.Json;

namespace FabSandbox.Data.Services
{
    public class DeviceServices : IDeviceServices
    {
        private readonly IDialogService _dialogService;
        private readonly IUserInterfaceToolsServices _userInterfaceToolsServices;
        private readonly IWebSocketService _webSocketService;
        private DeviceModel _deviceModel;

        public bool IsConnected => _webSocketService.IsConnected;
        public event Action? OnStateChanged;
        public event Action<DeviceMessage>? OnDataReceived;

        public DeviceServices(IDialogService dialogService,
                              IUserInterfaceToolsServices userInterfaceToolsServices,
                              IWebSocketService webSocketService)
        {
            _dialogService = dialogService;
            _userInterfaceToolsServices = userInterfaceToolsServices;
            _webSocketService = webSocketService;
            //_webSocketService.OnConnectionStateChanged += OnStateChanged;
            _webSocketService.OnLedDataReceived += HandleInputData;
            _deviceModel = new DeviceModel();

        }

        public async Task GetConfigData()
        {
            //Call values from device
            await SendDeviceMessageAsync(SystemConstants.MessageType.GetGPIO);

            await SendDeviceMessageAsync(SystemConstants.MessageType.GetInterval);

            await SendDeviceMessageAsync(SystemConstants.MessageType.GetValue);
        }

        public void SetDeviceModel(string deviceName, string deviceKey)
        {
            _deviceModel = new DeviceModel()
            {
                DeviceKey = deviceKey,
                DeviceName = deviceName,
            };
        }

        /// <summary>
        /// Handle message in, apply specific functions for device
        /// </summary>
        /// <param name="msg"></param>
        void HandleInputData(DeviceMessage msg)
        {
            try
            {
                if (msg.Device == _deviceModel.DeviceKey)
                {
                    switch (msg.Type)
                    {
                        case SystemConstants.MessageType.SetGPIO:
                            SetGPIOValue(int.Parse(msg.Value?.ToString()));
                            break;
                        case SystemConstants.MessageType.SetInterval:
                            SetNotifyIntervalValue(int.Parse(msg.Value?.ToString()));
                            break;
                        case SystemConstants.MessageType.Message:
                            ShowMessage(msg.Value.ToString());
                            break;
                        default:
                            OnDataReceived?.Invoke(msg);
                            break;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                _userInterfaceToolsServices.ShowSnackbarMessage(_deviceModel.DeviceName, $"{MessageConstants.ErrorMessage} - {ex.Message}", Severity.Warning);
            }
            catch (Exception)
            {
                _userInterfaceToolsServices.ShowSnackbarMessage(_deviceModel.DeviceName, $"{MessageConstants.ErrorMessage}", Severity.Error);
            }

            // StateHasChanged();
        }
            
        /// <summary>
        /// Send messages to ESP using required format
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SendDeviceMessageAsync(string messageType, object? value = null)
        {
            try
            {
                var message = new DeviceMessage()
                {
                    Device = _deviceModel.DeviceKey,
                    Type = messageType,
                    Value = value
                };

                var json = JsonSerializer.Serialize(message);

                await _webSocketService.SendAsync(json);
            }
            catch (ArgumentException ex)
            {
                _userInterfaceToolsServices.ShowSnackbarMessage(_deviceModel.DeviceName, $"{MessageConstants.ErrorMessage} al enviar el mensaje - {ex.Message}", Severity.Warning);
            }
            catch (Exception)
            {
                _userInterfaceToolsServices.ShowSnackbarMessage(_deviceModel.DeviceName, $"{MessageConstants.ErrorMessage} al enviar el mensaje", Severity.Error);
            }
        }

        /// <summary>
        /// Set the local GPIO value received from device
        /// </summary>
        /// <param name="gpio"></param>
        public void SetGPIOValue(int gpio)
        {
            _deviceModel.GpioCurrentValue = gpio;

            _userInterfaceToolsServices.ShowSnackbarMessage(_deviceModel.DeviceName, $"{_deviceModel.DeviceKey} {MessageConstants.HasSend} Set GPIO: {_deviceModel.GpioCurrentValue}", Severity.Info);

            OnStateChanged.Invoke();
        }

        /// <summary>
        /// Set the device GPIO value on the device
        /// </summary>
        /// <returns></returns>
        public async Task SetGPIOValueShowDialog()
        {
            await SetValueDialog(SystemConstants.MessageType.SetGPIO, "Set GPIO", "PIN", _deviceModel.GpioDefaultValue);
        }
        
        /// <summary>
        /// Set the local Notification Interval value received from device
        /// </summary>
        /// <param name="interval"></param>
        public void SetNotifyIntervalValue(int interval)
        {
            _deviceModel.NotifyInvervalValue = interval;

            _userInterfaceToolsServices.ShowSnackbarMessage(_deviceModel.DeviceName, $"{_deviceModel.DeviceKey} {MessageConstants.HasSend} Intervalo de Notificaciones: {_deviceModel.NotifyInvervalValue}", Severity.Info);

            OnStateChanged.Invoke();
        }

        /// <summary>
        /// Set the Notification Interval value on the device
        /// </summary>
        /// <returns></returns>
        public async Task SetNotifyIntervalValueShowDialog()
        {
            await SetValueDialog(SystemConstants.MessageType.SetInterval, "Set Interval", "ms", _deviceModel.NotifyInvervalDefaultValue);
        }

        /// <summary>
        /// Show the generic Set Value Control to send this to the device
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="title"></param>
        /// <param name="units"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetValueDialog(string messageType, string title, string units, int value)
        {
            var parameters = new DialogParameters();
            parameters.Add("Label", $"Establecer Valor");
            parameters.Add("Units", units);
            parameters.Add("Value", value);

            var options = new DialogOptions { CloseOnEscapeKey = true };

            var dialog = _dialogService.Show<SetValueDialogComponent>(title, parameters, options);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                try
                {
                    var message = new DeviceMessage()
                    {
                        Device = _deviceModel.DeviceKey,
                        Type = messageType,
                        Value = result.Data
                    };

                    var json = JsonSerializer.Serialize(message);

                    await _webSocketService.SendAsync(json);

                    _userInterfaceToolsServices.ShowSnackbarMessage($"{_deviceModel.DeviceName}", $"{MessageConstants.MessageSend} - {MessageConstants.MessageType}: {messageType} - {MessageConstants.Value}: {result.Data}", Severity.Success);

                }
                catch (ArgumentException ex)
                {
                    _userInterfaceToolsServices.ShowSnackbarMessage($"{_deviceModel.DeviceName}", $"{MessageConstants.ErrorMessage} al enviar el mensaje: {ex.Message}", Severity.Warning);
                }
                catch (Exception)
                {
                    _userInterfaceToolsServices.ShowSnackbarMessage($"{_deviceModel.DeviceName}", $"{MessageConstants.ErrorMessage} al enviar el mensaje", Severity.Error);
                }
            }
        }

        public void ShowMessage(string messageText)
        {
            _userInterfaceToolsServices.ShowSnackbarMessage($"{_deviceModel.DeviceName}", $"{MessageConstants.MessageReceived}: {messageText}", Severity.Info);

        }

        public int GetGPIOValue()
        {
            return _deviceModel.GpioCurrentValue;
        }

        public int GPIOValue
        {
            get
            {
                return _deviceModel.GpioCurrentValue;
            }
        }

        public int GetNotifyValue()
        {
            return _deviceModel.NotifyInvervalValue;
        }

        public string DeviceName
        {
            get { return _deviceModel.DeviceName; }

        }

        public string DeviceKey
        {
            get { return _deviceModel.DeviceKey; }

        }

        public DeviceModel DeviceModel
        {
            get
            {
                return _deviceModel;
            }
        }
    }
}
