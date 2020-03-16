using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Hardware;

namespace Tetris
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        const int pulseDuration = 3000;
        RgbPwmLed rgbPwmLed;

        Ssd1309 display;
        GraphicsLibrary graphics;

        /*
        IDigitalInterruptPort portLeft;
        IDigitalInterruptPort portUp;
        IDigitalInterruptPort portRight;
        IDigitalInterruptPort portDown;
        */

        IDigitalInputPort portLeft;
        IDigitalInputPort portUp;
        IDigitalInputPort portRight;
        IDigitalInputPort portDown;

        TetrisGame game = new TetrisGame(8, 16);

        public MeadowApp()
        {
            Console.WriteLine("Tetris");

            Init();

            graphics.Clear();
            graphics.DrawText(0, 0, "Meadow Tetris");
            graphics.DrawText(0, 10, "v0.0.1");
            graphics.Show();

            Thread.Sleep(1000);

            StartGameLoop();

            for(int i = 0; i < 30; i++)
            {
                DrawTetrisField();
                game.Reset();
                Thread.Sleep(500);
            }

            Thread.Sleep(2000);

            Console.WriteLine("Count");

            PulseRgbPwmLed();


            /*  for (int i = 0; i < 9999; i++)
              {
                  display.Clear();
                  graphics.DrawText(0, 0, $"{i}");

                  display.Show();
              }

              PulseRgbPwmLed(); */
        }

        bool playing = true;
        int tick = 0;
        void StartGameLoop()
        {
            while(playing)
            {
                tick++;
                if(tick % 15 == 0)
                {
                    game.OnDown(true);
                }

                if(portLeft.State == true)
                {
                //    Console.WriteLine("Left");
                    game.OnLeft();
                }
                else if(portRight.State == true)
                {
                 //   Console.WriteLine("Right");
                    game.OnRight();
                }
                else if(portUp.State == true)
                {
                  //  Console.WriteLine("Rotate");
                    game.OnRotate();
                }
                else if(portDown.State == true)
                {
                 //   Console.WriteLine("Down");
                    game.OnDown();
                }

                graphics.Clear();
                DrawTetrisField();
                graphics.Show();

                Thread.Sleep(50);
            }
        }

        int scale = 8;
        void DrawTetrisField()
        {
         //   graphics.Clear();

            //draw current piece
            for(int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if(game.IsPieceLocationSet(i, j))
                    {
                        //  graphics.DrawPixel(i, j);
                        graphics.DrawRectangle((game.CurrentPiece.X + i) * scale,
                            (game.CurrentPiece.Y + j) * scale,
                            scale + 1, scale, true, true);//+1 hack until we fix the graphics lib
                    }
                }
            }

            //draw gamefield
            for (int i = 0; i < game.Width; i++)
            {
                for (int j = 0; j < game.Height; j++)
                {
                    if (game.IsGameFieldSet(i, j))
                    {
                        graphics.DrawRectangle((i) * scale,
                            (j) * scale,
                            scale + 1, scale, true, true);//+1 hack until we fix the graphics lib
                    }
                }
            } 
        }

        void Init()
        {
            Console.WriteLine("Init");

            portLeft = Device.CreateDigitalInputPort(Device.Pins.D12);
            portUp = Device.CreateDigitalInputPort(Device.Pins.D13);
            portRight = Device.CreateDigitalInputPort(Device.Pins.D07);
            portDown = Device.CreateDigitalInputPort(Device.Pins.D11);

            /*   portLeft = Device.CreateDigitalInputPort(Device.Pins.D12, InterruptMode.EdgeFalling, ResistorMode.PullDown);
               portUp = Device.CreateDigitalInputPort(Device.Pins.D13, InterruptMode.EdgeFalling, ResistorMode.PullDown);
               portRight = Device.CreateDigitalInputPort(Device.Pins.D07, InterruptMode.EdgeFalling, ResistorMode.PullDown);
               portDown = Device.CreateDigitalInputPort(Device.Pins.D11, InterruptMode.EdgeFalling, ResistorMode.PullDown);

               portRight.Changed += PortRight_Changed;
               portLeft.Changed += PortLeft_Changed;
               portUp.Changed += PortUp_Changed;
               portDown.Changed += PortDown_Changed; */

            rgbPwmLed = new RgbPwmLed(Device,
                       Device.Pins.OnboardLedRed,
                       Device.Pins.OnboardLedGreen,
                       Device.Pins.OnboardLedBlue);

            var config = new Meadow.Hardware.SpiClockConfiguration(12000, Meadow.Hardware.SpiClockConfiguration.Mode.Mode0);

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
            graphics.CurrentFont = new Font4x8();
            graphics.Rotation = GraphicsLibrary.RotationType._270Degrees;
        }

        private void PortRight_Changed(object sender, DigitalInputPortEventArgs e)
        {
            Console.WriteLine("OnRight");
            game.OnRight();
        }

        private void PortDown_Changed(object sender, DigitalInputPortEventArgs e)
        {
            Console.WriteLine("OnDown");
        }

        private void PortUp_Changed(object sender, DigitalInputPortEventArgs e)
        {
        //    Console.WriteLine("OnUp");
        }

        private void PortLeft_Changed(object sender, DigitalInputPortEventArgs e)
        {
            Console.WriteLine("OnLeft");
            game.OnLeft();
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
