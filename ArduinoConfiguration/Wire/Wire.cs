using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConfiguration.Wire
{
    class Wire
    {
        public void wireBegin(Int16 delay)
        {
            byte[] message = new byte[5];
            message[0] = (byte)(0XF0);
            message[1] = (byte)(Settings.getSetting().I2C_CONFIG);
            message[2] = (byte)(delay & 0x7F);
            message[3] = (byte)(delay >> 7);
            message[4] = (byte)(Settings.getSetting().END_SYSEX);//END_SYSEX
            Settings.getSetting()._serialPort.Write(message, 0, 5);
        }
       
        public void wireRequest(byte slaveAddress, Int16 slaveRegister, Int16[] data, byte mode, SerialPort _serialPort)
        {
            byte[] message = new byte[Settings.getSetting().MAX_DATA_BYTES];
            message[0] = (byte)(0xF0);
            message[1] = (byte)(Settings.getSetting().I2C_REQUEST);
            message[2] = (byte)(slaveAddress);
            message[3] = (byte)(mode);
            int index = 4;
            if (slaveRegister != Arduino.NONE)
            {
                message[index] = (byte)(slaveRegister & 0x7F);
                index += 1;
                message[index] = (byte)(slaveRegister >> 7);
                index += 1;
            }
            for (int i = 0; i < (data.Count()); i++)
            {
                message[index] = (byte)(data[i] & 0x7F);
                index += 1;
                message[index] = (byte)(data[i] >> 7);
                index += 1;
            }
            message[index] = (byte)(Settings.getSetting().END_SYSEX);
            _serialPort.Write(message, 0, index + 1);
        }
    }
}
