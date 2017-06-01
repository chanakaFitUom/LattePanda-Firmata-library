using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConfiguration.Servo
{
    class Servo 
    {
        public void servoWrite(int pin, int angle)
        {
            byte[] message = new byte[3];
            message[0] = (byte)(Settings.ANALOG_MESSAGE | (pin & 0x0F));
            message[1] = (byte)(angle & 0x7F);
            message[2] = (byte)(angle >> 7);
            Settings.getSetting()._serialPort.Write(message, 0, 3);
        }
    }
}
