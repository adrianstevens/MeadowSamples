using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Hardware;

namespace Connect4
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;

        Ssd1309 display;
        GraphicsLibrary graphics;

        IDigitalInputPort portLeft;
        IDigitalInputPort portRight;
        IDigitalInputPort portDown;

        Connect4Game connectGame;

        byte currentColumn = 0;

        public MeadowApp()
        {
            Console.WriteLine("Connect4");

            connectGame = new Connect4Game();

            Initialize();

            graphics.Clear();
            graphics.DrawText(0, 0, "Meadow Connect4");
            graphics.DrawText(0, 10, "v0.0.1");
            graphics.Show();

            Thread.Sleep(500);

            StartGameLoop();
        }

        void StartGameLoop()
        {
            while (true)
            {
                CheckInput();

                graphics.Clear();
                DrawGame();
                graphics.Show();

                Thread.Sleep(50);
            }
        }

        void CheckInput()
        {
            if (portLeft.State == true)
            {
                if(currentColumn > 0)
                {
                    currentColumn -= 1;
                }
            }
            else if (portRight.State == true)
            {
                if (currentColumn < connectGame.Width - 1)
                {
                    currentColumn += 1;
                }
            }
            else if (portDown.State == true)
            {
                connectGame.AddChip(currentColumn);
            }
        }

        void DrawGame()
        {

            graphics.DrawText(0, 0, $"status");

            graphics.DrawRectangle(6, 10, 52, 112);
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);

            portLeft = Device.CreateDigitalInputPort(Device.Pins.D12);
            portRight = Device.CreateDigitalInputPort(Device.Pins.D07);
            portDown = Device.CreateDigitalInputPort(Device.Pins.D11);

            var config = new SpiClockConfiguration(12000, SpiClockConfiguration.Mode.Mode0);

            var bus = Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config);

            display = new Ssd1309
            (
                device: Device,
                spiBus: bus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00
            );

            graphics = new GraphicsLibrary(display);
            graphics.CurrentFont = new Font8x12();
        }
    }
}