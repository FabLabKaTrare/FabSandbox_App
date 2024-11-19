namespace FabSandbox.Data.Config
{
    public static class SystemConstants
    {
        public struct DeviceKey
        {
            public const string Button = "button";
            public const string DHT = "dht";
            public const string LED = "led";
            public const string LDR = "ldr";
            public const string Pixels = "pixel";
            public const string Potentiometer = "pot";
            public const string Relay = "relay";
            public const string Servo = "servo";
            public const string Stepper = "stepper";

        }

        public struct MessageType
        {
            public const string Message = "message";

            public const string GetValue = "get_value";
            public const string SetValue = "set_value";

            public const string GetInterval = "get_interval";
            public const string SetInterval = "set_interval";

            public const string GetGPIO = "get_gpio";
            public const string SetGPIO = "set_gpio";

            public const string SetReadAndNotify = "set_readAndNotify";

            public const string SetBrightness = "set_brightness";

            public const string SetOnCancel = "set_cancel";
        }


    }
}
