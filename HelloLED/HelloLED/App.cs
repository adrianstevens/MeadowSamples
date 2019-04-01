using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.Lcd;
using Meadow.Hardware;

namespace HelloLED
{
    class App : AppBase<F7Micro, App>
    {
        private IDigitalOutputPort redLed;
        private IDigitalOutputPort blueLed;
        private IDigitalOutputPort greenLed;
        private Lcd2004 display;

        public App ()
        {
            InitHardware();
            ToggleLeds();
        }

        public void InitHardware()
        {
            redLed =  Device.CreateDigitalOutputPort(Device.Pins.OnboardLEDRed, false);
        //    blueLed = new DigitalOutputPort(Device.Pins.OnboardLEDBlue, false);
      //      greenLed = new DigitalOutputPort(Device.Pins.OnboardLEDGreen, false);

            display = new Lcd2004(App.Device, Device.Pins.D05, Device.Pins.D07,
                Device.Pins.D08, Device.Pins.D09, Device.Pins.D10, Device.Pins.D11, 2, 16);
        }

        public void ToggleLeds()
        {
            var state = false;

            while(true)
            {
                state = !state;

                Console.WriteLine($"State: {state}");
                display.ClearLine(0);
                display.WriteLine($"State: {state}", 0);

                redLed.State = state;
                display.ClearLine(1);
                display.WriteLine("Red LED is " + (state ? "On" : "Off"), 1);
                Thread.Sleep(200);

                /*
                greenLed.State = state;
                display.ClearLine(2);
                display.WriteLine("Green LED is " + (state ? "On" : "Off"), 2);
                Thread.Sleep(200);

                blueLed.State = state;
                display.ClearLine(3);
                display.WriteLine("Blue LED is " + (state ? "On" : "Off"), 3);
                Thread.Sleep(200);*/
            }
        }
    }
}