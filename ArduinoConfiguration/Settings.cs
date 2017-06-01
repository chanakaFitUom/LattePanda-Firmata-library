using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoConfiguration
{
   public class Settings
    {


        public int MAX_DATA_BYTES { get; private set; }
        public int TOTAL_PORTS { get; private set; }
        public int SERVO_CONFIG { get; private set; }// set max angle, minPulse, maxPulse, freq

        public int REPORT_DIGITAL { get; private set; } // enable digital input by port
        public int SET_PIN_MODE { get; private set; } // set a pin to INPUT/OUTPUT/PWM/etc
        public int REPORT_ANALOG { get; private set; } // enable analog input by pin #
        public int SYSTEM_RESET { get; private set; } // reset from MIDI
        public int START_SYSEX { get; private set; } // start a MIDI SysEx message
        public int END_SYSEX { get; private set; } // end a MIDI SysEx message
        public int I2C_REQUEST { get; private set; } // I2C request messages from a host to an I/O board
        public int I2C_REPLY { get; private set; } // I2C reply messages from an I/O board to a host
        public int I2C_CONFIG { get; private set; }// Configure special I2C settings such as power pins and delay times




        public int _waitForData;
        public int _executeMultiByteCommand;
        public int _multiByteChannel;
        public int[] _storedInputData;

        public volatile int[] _digitalOutputData = new int[64];
        public volatile int[] _digitalInputData = new int[64];
        public volatile int[] _analogInputData = new int[64];
        public int _majorVersion;
        public int _minorVersion;

        public SerialPort _serialPort;
        public bool _parsingSysex;
        public int _sysexBytesRead;
        public Thread _readThread;
        public object _locker;
        public int _delay; 

        public  const int DIGITAL_MESSAGE = 0x90; // send data for a digital port
        public  const int ANALOG_MESSAGE = 0xE0; // send data for an analog pin (or PWM)
        public  const int REPORT_VERSION = 0xF9; // report firmware version

        

        //Create Singleton object
        private static Settings obj = new Settings(); //Early, instance will be created at load time  

       //Constructor
        public Settings()
        {
            MAX_DATA_BYTES = 64;
            TOTAL_PORTS = 2;
            SERVO_CONFIG = 0x70;
            REPORT_DIGITAL = 0xD0;
            SET_PIN_MODE = 0xF4;
            REPORT_ANALOG = 0xC0;
            SYSTEM_RESET = 0xFF;
            START_SYSEX = 0xF0;
            END_SYSEX = 0xF7;
            I2C_REQUEST = 0x76;
            I2C_REPLY = 0x77;
            I2C_CONFIG = 0x78;

            _waitForData = 0;
            _executeMultiByteCommand = 0;
            _multiByteChannel = 0;
            _storedInputData = new int[64];

            

            _majorVersion = 0;
            _minorVersion = 0;

            _readThread = null;
            _locker = new object();
            
        }
        //return Singleton object
        public static Settings getSetting()
        {
            //if object already exist
            if (obj != null) {
                return obj;
            }
            else
            {
                return new Settings();
            }
          
         }
    }
}
