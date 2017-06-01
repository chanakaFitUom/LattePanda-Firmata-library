using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConfiguration
{
    class PinModeConfig
    {
        public void pinMode(int pin, byte mode, SerialPort _serialPort)
        {
            
            byte[] message = new byte[3];
            message[0] = (byte)(Settings.getSetting().SET_PIN_MODE);
            message[1] = (byte)(pin);
            message[2] = (byte)(mode);
            Settings.getSetting()._serialPort.Write(message, 0, 3);
            message = null;
        }
    }
}
