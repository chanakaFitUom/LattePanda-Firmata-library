using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConfiguration.Numbers
{
    class Numbers
    {
        Arduino ard = new Arduino();


        /*Blinking a Number
            Back in the dawn age of computing sometimes things went wrong enough that the only way to
            communicate an error to the user was to blink a light. When dealing with micro-controllers
            this can sometimes come in handy. The following function blinks a sequence to indicate a
            number visually.
            */

        void blinkNumber(String numString)
        {
            int versLength = numString.Length;
            ard.getSettings()._delay = 200;

            for (int i = 0; i < versLength; i++)
            {
                int number = numString[i] - 48;
                if (number == 0)
                {
                    //pin 5 used as OUTPUT pin
                    ard.setPinMode().pinMode(5, Arduino.OUTPUT, ard.getSettings()._serialPort);
                    ard.getLED().blinkLED(5, 1, 20);
                    ard.getSettings()._delay = 160;
                    
                }
                if (number > 0 && number < 10) ard.getLED().blinkLED(5, number, 200);
                ard.getSettings()._delay = 400;
            }
        }






    }
}
