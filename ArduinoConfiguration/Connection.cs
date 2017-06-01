using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;


namespace ArduinoConfiguration
{
    class Connection
    {

        Arduino ard = new Arduino();


        
        Event.DidI2CDataReveive didI2CDataReveive;
        Event.AnalogPinUpdated analogPinUpdated;
        Event.DigitalPinUpdated digitalPinUpdated;


        //Get Singleton settings object
        //Settings setting = Settings.getSetting();
        

      
        public void processInput()
        {
           

            while (ard.getSettings()._serialPort.IsOpen)
            {


                if (ard.getSettings()._serialPort.BytesToRead > 0)
                {
                    lock (this)
                    {
                        int inputData = ard.getSettings()._serialPort.ReadByte();
                        int command;

                        if (ard.getSettings()._parsingSysex)
                        {
                            if (inputData == ard.getSettings().END_SYSEX)
                            {
                                ard.getSettings()._parsingSysex = false;
                                if (ard.getSettings()._sysexBytesRead > 5 && ard.getSettings()._storedInputData[0] == ard.getSettings().I2C_REPLY)
                                {
                                    byte[] i2cReceivedData = new byte[(ard.getSettings()._sysexBytesRead - 1) / 2];
                                    for (int i = 0; i < i2cReceivedData.Count(); i++)
                                    {
                                        i2cReceivedData[i] = (byte)(ard.getSettings()._storedInputData[(i * 2) + 1] | ard.getSettings()._storedInputData[(i * 2) + 2] << 7);
                                    }
                                    if (this.didI2CDataReveive != null  )
                                        didI2CDataReveive(i2cReceivedData[0], i2cReceivedData[1], i2cReceivedData.Skip(2).ToArray());
                                    
                                }
                                ard.getSettings()._sysexBytesRead = 0;
                            }
                            else
                            {
                                ard.getSettings()._storedInputData[ard.getSettings()._sysexBytesRead] = inputData;
                                ard.getSettings()._sysexBytesRead++;
                            }
                        }
                        else if (ard.getSettings()._waitForData > 0 && inputData < 128)
                        {
                            ard.getSettings()._waitForData--;
                            ard.getSettings()._storedInputData[ard.getSettings()._waitForData] = inputData;

                            if (ard.getSettings()._executeMultiByteCommand != 0 && ard.getSettings()._waitForData == 0)
                            {

                                switch (ard.getSettings()._executeMultiByteCommand)
                                {

                                    case Settings.DIGITAL_MESSAGE:
                                        int currentDigitalInput = (ard.getSettings()._storedInputData[0] << 7) + ard.getSettings()._storedInputData[1];
                                        for (int i = 0; i < 8; i++)
                                        {
                                            if (((1 << i) & (currentDigitalInput & 0xff)) != ((1 << i) & (ard.getSettings()._digitalInputData[ard.getSettings()._multiByteChannel] & 0xff)))
                                            {
                                                if ((((1 << i) & (currentDigitalInput & 0xff))) != 0)
                                                {
                                                    if (this.digitalPinUpdated != null)
                                                        this.digitalPinUpdated((byte)(i + ard.getSettings()._multiByteChannel * 8), Arduino.HIGH);
                                                }
                                                else
                                                {
                                                    if (this.digitalPinUpdated != null)
                                                        this.digitalPinUpdated((byte)(i + ard.getSettings()._multiByteChannel * 8), Arduino.LOW);
                                                }
                                            }
                                        }
                                        ard.getSettings()._digitalInputData[ard.getSettings()._multiByteChannel] = (ard.getSettings()._storedInputData[0] << 7) + ard.getSettings()._storedInputData[1];

                                        break;
                                    case Settings.ANALOG_MESSAGE:
                                        ard.getSettings()._analogInputData[ard.getSettings()._multiByteChannel] = (ard.getSettings()._storedInputData[0] << 7) + ard.getSettings()._storedInputData[1];
                                        if (this.analogPinUpdated != null)
                                            analogPinUpdated(ard.getSettings()._multiByteChannel, (ard.getSettings()._storedInputData[0] << 7) + ard.getSettings()._storedInputData[1]);
                                        break;
                                    case Settings.REPORT_VERSION:
                                        this.ard.getSettings()._majorVersion = ard.getSettings()._storedInputData[1];
                                        this.ard.getSettings()._minorVersion = ard.getSettings()._storedInputData[0];
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (inputData < 0xF0)
                            {
                                command = inputData & 0xF0;
                                ard.getSettings()._multiByteChannel = inputData & 0x0F;
                                switch (command)
                                {
                                    case Settings.DIGITAL_MESSAGE:
                                    case Settings.ANALOG_MESSAGE:
                                    case Settings.REPORT_VERSION:
                                        ard.getSettings()._waitForData = 2;
                                        ard.getSettings()._executeMultiByteCommand = command;
                                        break;
                                }
                            }
                            else if (inputData == 0xF0)
                            {
                                ard.getSettings()._parsingSysex = true;
                                
                            }

                        }
                    }
                }
            }
        }



        public void Open()
        {
            ard.getSettings()._serialPort.DtrEnable = true;
            ard.getSettings()._serialPort.Open();

            Thread.Sleep(ard.getSettings()._delay);

            byte[] command = new byte[2];

            for (int i = 0; i < 6; i++)
            {
                command[0] = (byte)(ard.getSettings().REPORT_ANALOG | i);
                command[1] = (byte)1;
                ard.getSettings()._serialPort.Write(command, 0, 2);
            }

            for (int i = 0; i < 2; i++)
            {
                command[0] = (byte)(ard.getSettings().REPORT_DIGITAL | i);
                command[1] = (byte)1;
                ard.getSettings()._serialPort.Write(command, 0, 2);
            }
            command = null;

            if (ard.getSettings()._readThread == null)
            {
                ard.getSettings()._readThread = new Thread(processInput);
                ard.getSettings()._readThread.Start();
            }
        }
       
        public void Close()
        {
            ard.getSettings()._readThread.Join(500);
            ard.getSettings()._readThread = null;
            ard.getSettings()._serialPort.Close();
        }


    }




    
}
