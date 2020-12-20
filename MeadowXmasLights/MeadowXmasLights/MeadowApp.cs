﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using System;
using System.Threading;

namespace MeadowXmasLights
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Apa102 ledStrip;

        public MeadowApp()
        {
            Initialize();

            while(true)
            {
                WalkXmasColors();

                Console.WriteLine("Cycle colors");
                CycleColors(5000);

                WalkColors(Color.Red);
                WalkColors(Color.Green);
                WalkColors(Color.Blue);
                WalkColors(Color.Yellow);
                WalkColors(Color.Cyan);
                WalkColors(Color.Violet);
            }
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");
            ISpiBus spiBus = Device.CreateSpiBus();
            IDigitalOutputPort spiPeriphChipSelect = Device.CreateDigitalOutputPort(Device.Pins.D04);
            ledStrip = new Apa102(spiBus, spiPeriphChipSelect, 144, Apa102.PixelOrder.BGR);
        }

        void WalkXmasColors(int count = 100)
        {
            int offset = 0;
            Color color;

            while (count > 0)
            {
                if (offset % 4 == 0)
                {
                    color = Color.Red;
                }
                else if (offset % 4 == 1)
                {
                    color = Color.Blue;
                }
                else if (offset % 4 == 2)
                {
                    color = Color.Green;
                }
                else//f (offset % 4 == 1)
                {
                    color = Color.Yellow;
                }

                for (uint i = 0; i < ledStrip.NumberOfLeds; i++)
                {
                    if ((offset + i) % 4 == 0)
                    {
                        ledStrip.SetLed(i, color);
                    }
                    else
                    {
                        ledStrip.SetLed(i, Color.Black);
                    }
                }

                ledStrip.Show();

                Thread.Sleep(400);

                offset++;
                count--;
            }
        }

        void WalkColors(Color color, int count = 25)
        {
            int offset = 0;

            while(count > 0)
            {
                for (uint i = 0; i < ledStrip.NumberOfLeds; i++)
                {
                    if ((offset + i) % 5 == 0)
                    {
                        ledStrip.SetLed(i, color);
                    }
                    else
                    {
                        ledStrip.SetLed(i, Color.Black);
                    }
                }

                ledStrip.Show();

                Thread.Sleep(400);

                offset++;
                count--;
            }
        }

        void CycleColors(int duration)
        {
            Console.WriteLine("Cycle colors...");
            
            ShowColorPulse(Color.Blue, duration);
            ShowColorPulse(Color.Cyan, duration);
            ShowColorPulse(Color.Green, duration);
            ShowColorPulse(Color.GreenYellow, duration);
            ShowColorPulse(Color.Yellow, duration);
            ShowColorPulse(Color.Orange, duration);
            ShowColorPulse(Color.OrangeRed, duration);
            ShowColorPulse(Color.Red, duration);
            ShowColorPulse(Color.MediumVioletRed, duration);
            ShowColorPulse(Color.Purple, duration);
        }

        void ShowColorPulse(Color color, int duration = 1000)
        {
            float brightness = 0.05f;
            bool forward = true;

            DateTime time = DateTime.Now;

            while ((DateTime.Now - time).TotalMilliseconds < duration)
            {
                for (uint i = 0; i < ledStrip.NumberOfLeds; i++)
                {
                    ledStrip.SetLed(i, color, brightness);
                }

                if (forward) { brightness += 0.01f; }
                else { brightness -= 0.01f; }

                if (brightness <= 0.05f)
                {
                    forward = true;
                }
                if (brightness >= 0.5f)
                {
                    forward = false;
                }

                ledStrip.Show();
            }
        }
    }
}