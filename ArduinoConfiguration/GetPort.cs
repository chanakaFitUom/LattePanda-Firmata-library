using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConfiguration
{
   public class GetPort
    {
        public static string[] list()
        {
            return SerialPort.GetPortNames();
        }
    }
}
