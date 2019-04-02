using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.Lcd;
using Meadow.Foundation.Sensors.Switches;
using Meadow.Hardware;

namespace HelloLED
{
    class App : AppBase<F7Micro, App>
    {
        private SpdtSwitch spdtSwitch;
        private IDigitalOutputPort redLed;
        private IDigitalOutputPort blueLed;
        private IDigitalOutputPort greenLed;
        private Lcd2004 display;
        private int count;

        public App ()
        {
            InitHardware();
        //    ToggleLeds();
        }

        public void InitHardware()
        {
            redLed =  Device.CreateDigitalOutputPort(Device.Pins.OnboardLEDRed, false);
            blueLed = Device.CreateDigitalOutputPort(Device.Pins.OnboardLEDBlue, false);
            greenLed = Device.CreateDigitalOutputPort(Device.Pins.OnboardLEDGreen, false);

            spdtSwitch = new SpdtSwitch(Device, Device.Pins.D14, InterruptMode.EdgeBoth, ResistorMode.Disabled); 
            spdtSwitch.Changed += SpdtSwitch_Changed;

            display = new Lcd2004(App.Device, Device.Pins.D05, Device.Pins.D07,
                Device.Pins.D08, Device.Pins.D09, Device.Pins.D10, Device.Pins.D11, 2, 16);

            blueLed.State = true;
        }

        void SpdtSwitch_Changed(object sender, EventArgs e)
        {
            display.ClearLine(0);
            display.WriteLine("Switch is " + (spdtSwitch.IsOn ? "On" : "Off"), 0);

            display.ClearLine(1);
            display.WriteLine($"Count: {count++}", 1);

            redLed.State = spdtSwitch.IsOn;
            greenLed.State = !spdtSwitch.IsOn;
        }

     
    }
}