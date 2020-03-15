using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;

namespace Tetris
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        const int pulseDuration = 3000;
        RgbPwmLed rgbPwmLed;

        Ssd1309 display;
        GraphicsLibrary graphics;

        public MeadowApp()
        {
            Init();

            graphics.Clear();
            graphics.DrawText(0, 0, "Hello");
            graphics.Show();

            PulseRgbPwmLed();
        }

        void Init()
        {
            rgbPwmLed = new RgbPwmLed(Device,
                       Device.Pins.OnboardLedRed,
                       Device.Pins.OnboardLedGreen,
                       Device.Pins.OnboardLedBlue);

            display = new Ssd1309(Device, Device.CreateSpiBus(), Device.Pins.D02, Device.Pins.D01, Device.Pins.D00);

            graphics = new GraphicsLibrary(display);
            graphics.CurrentFont = new Font12x16();


        }

        protected void PulseRgbPwmLed()
        {
            while (true)
            {
                Pulse(Color.Red);
                Pulse(Color.Green);
                Pulse(Color.Blue);
            }
        }

        protected void Pulse(Color color)
        {
            rgbPwmLed.StartPulse(color);
            Console.WriteLine($"Pulsing {color}");
            Thread.Sleep(pulseDuration);
            rgbPwmLed.Stop();
        }
    }
}
