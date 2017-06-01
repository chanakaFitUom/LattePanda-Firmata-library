using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConfiguration.Digital
{
    class Digital
    {

        public int digitalRead(int pin)
        {

            return ((Settings.getSetting()._digitalInputData[pin >> 3] >> (pin & 0x07)) & 0x01);
        }
        public void digitalWrite(int pin, byte value)
        {
            int portNumber = (pin >> 3) & 0x0F;
            byte[] message = new byte[3];

            if ((int)value == 0)
                Settings.getSetting()._digitalOutputData[portNumber] &= ~(1 << (pin & 0x07));
            else
                Settings.getSetting()._digitalOutputData[portNumber] |= (1 << (pin & 0x07));

            message[0] = (byte)(Settings.DIGITAL_MESSAGE | portNumber);
            message[1] = (byte)(Settings.getSetting()._digitalOutputData[portNumber] & 0x7F);
            message[2] = (byte)(Settings.getSetting()._digitalOutputData[portNumber] >> 7);
            Settings.getSetting()._serialPort.Write(message, 0, 3);
        }
    }
}
