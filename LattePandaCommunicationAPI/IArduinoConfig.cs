using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LattePandaCommunicationAPI
{
   public interface IArduinoConfig
    {
        public abstract void servoWrite(int pin, int angle);
    }
}
