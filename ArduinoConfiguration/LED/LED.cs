using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArduinoConfiguration.LED
{
    class LED
    {
        Arduino ard = new Arduino();


        /*  Blinking an LED
            This function blinks an LED light as many times as requested, at the requested blinking rate.
        */
        public void blinkLED(int targetPin, int numBlinks, int blinkRate)
        {
            for (int i = 0; i < numBlinks; i++)
            {
                //set target pin to OUTPUT
                ard.setPinMode().pinMode(targetPin, Arduino.OUTPUT, ard.getSettings()._serialPort);

                ard.getDigital().digitalWrite(targetPin, Arduino.HIGH);
                ard.getSettings()._delay = blinkRate;
                ard.getDigital().digitalWrite(targetPin, Arduino.LOW);
                ard.getSettings()._delay = blinkRate;
            }
        }




    }
}
