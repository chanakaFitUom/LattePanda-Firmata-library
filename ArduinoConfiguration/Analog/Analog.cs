using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConfiguration.Analog
{
    class Analog
    {
        public int analogRead(int pin)
        {
            return Settings.getSetting()._analogInputData[pin];
        }

        public void analogWrite(int pin, int value)
        {
            byte[] message = new byte[3];
            message[0] = (byte)(Settings.ANALOG_MESSAGE | (pin & 0x0F));
            message[1] = (byte)(value & 0x7F);
            message[2] = (byte)(value >> 7);
            Settings.getSetting()._serialPort.Write(message, 0, 3);
        }

    }
}
