
/* *************************************************************************************
Not nessary to setup  Arduino Connection
When Arduino Object create
Automatically setup the arduino connection  
****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

namespace ArduinoConfiguration
{
    class Arduino
    {
        public const byte LOW = 0, HIGH = 1, INPUT = 0, OUTPUT = 1, ANALOG = 2, PWM = 3, SERVO = 4,I2C_MODE_WRITE = 0x00,I2C_MODE_READ_ONCE = 0x08,I2C_MODE_READ_CONTINUOUSLY = 0x10, I2C_MODE_STOP_READING = 0x18;
        public const int NONE = -1;


        // ------------------------- private reference variables -------------------------------------------------------------


        private Settings setting = Settings.getSetting();
        private Connection con = new Connection();
        private GetPort port = new GetPort();
        private Event events = new Event();
        private PinModeConfig pinmode = new PinModeConfig();

        private Analog.Analog analog = new Analog.Analog();
        private Digital.Digital digital = new Digital.Digital();
        private Servo.Servo servo = new Servo.Servo();
        private Wire.Wire wire = new Wire.Wire();
        private LED.LED led = new LED.LED();
        private PinModeConfig pinMode = new PinModeConfig(); 


        //--------------------------------   Constructors    -------------------------------------------------------------------


        public Arduino(string serialPortName, Int32 baudRate, bool autoStart, int delay)
        {   
            setting._serialPort = new SerialPort(serialPortName, baudRate);
            setting._serialPort.DataBits = 8;
            setting._serialPort.Parity = Parity.None;
            setting._serialPort.StopBits = StopBits.One;

            if (autoStart)
            {
                setting._delay = delay;
                // connection open
                con.Open();
            }
        }
        public Arduino(string serialPortName) : this(serialPortName, 57600, true, 8000) { }
        public Arduino(string serialPortName, Int32 baudRate) : this(serialPortName, baudRate, true, 8000) { }
        public Arduino() : this(GetPort.list().ElementAt(GetPort.list().Length - 1), 57600, true, 8000) { }



        // analog functions
        public Analog.Analog getAnalog() {
            return this.analog;
        }

    

        // digital functions
        public Digital.Digital getDigital() {
            return this.digital;       
        }


        // servo functions
        public Servo.Servo getServo() {
            return this.servo;        
        }


        // wire functions
        public Wire.Wire getWire() {
            return this.wire;              
        }


        public Settings getSettings() {
            return this.setting;

        }

        public LED.LED getLED() {
            return this.led;
        }

        public PinModeConfig setPinMode() {
            return this.pinMode;
        }

        public Event getEvents() { 
            return this.events;
        }



    }
}
