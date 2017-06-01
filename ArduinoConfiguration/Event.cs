using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConfiguration
{
   public class Event
    {
        public delegate void AnalogPinUpdated(int pin, int value);
        public delegate void DidI2CDataReveive(byte address, byte register, byte[] data);
        public delegate void DigitalPinUpdated(byte pin, byte state);

        
    }
}
