using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.LCD;
using Meadow.Hardware;

namespace HelloLED
{
    class App : AppBase<F7Micro, App>
    {
        private DigitalOutputPort redLed;
        private DigitalOutputPort blueLed;
        private DigitalOutputPort greenLed;
        private Lcd2004 display;

        public override void Run()
        {
            InitHardware();
            ToggleLeds();
        }

        public void InitHardware()
        {
            redLed = new DigitalOutputPort(Device.Pins.OnboardLEDRed, false);
            blueLed = new DigitalOutputPort(Device.Pins.OnboardLEDBlue, false);
            greenLed = new DigitalOutputPort(Device.Pins.OnboardLEDGreen, false);

            display = new Lcd2004(Device.Pins.D05, Device.Pins.D07,
                Device.Pins.D11, Device.Pins.D12, Device.Pins.D13, Device.Pins.D14);
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

                greenLed.State = state;
                display.ClearLine(2);
                display.WriteLine("Green LED is " + (state ? "On" : "Off"), 2);
                Thread.Sleep(200);

                blueLed.State = state;
                display.ClearLine(3);
                display.WriteLine("Blue LED is " + (state ? "On" : "Off"), 3);
                Thread.Sleep(200);
            }
        }
    }
}