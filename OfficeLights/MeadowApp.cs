using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using System;
using System.Threading;
using Meadow.Peripherals.Leds;
using System.Numerics;

namespace MerryXmasLights
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Apa102 ledStrip;

        Color[] twinkleColors =
{
            Color.Green,
            Color.Red,
            Color.Blue,
            Color.Orange,
            Color.Indigo
        };

        public MeadowApp()
        {
            Initialize();

            Comet();

            Twinkle();

            while (true)
            {
                WalkXmasColors();

                Console.WriteLine("Cycle colors");
                CycleColors(5000);

                WalkColors(Color.Orange, ledStrip.NumberOfLeds);
                WalkColors(Color.Red,  ledStrip.NumberOfLeds);
                WalkColors(Color.Violet, ledStrip.NumberOfLeds);
                WalkColors(Color.Blue, ledStrip.NumberOfLeds);
                WalkColors(Color.Cyan, ledStrip.NumberOfLeds);
                WalkColors(Color.Green, ledStrip.NumberOfLeds);
                WalkColors(Color.Yellow, ledStrip.NumberOfLeds);
            }
        }

        void Initialize()
        {
            var onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                IRgbLed.CommonType.CommonAnode);

            onboardLed.SetColor(Color.Red);

            Console.WriteLine("Initialize hardware...");
            ISpiBus spiBus = Device.CreateSpiBus();
           /* IDigitalOutputPort spiPeriphChipSelect = 
                Device.CreateDigitalOutputPort(Device.Pins.D04);*/
            ledStrip = new Apa102(
                spiBus: spiBus,
                numberOfLeds: 101, 
                pixelOrder: Apa102.PixelOrder.BGR);

            onboardLed.SetColor(Color.Green);
        }

        //uses APA102 field named ledStrip
        void Comet()
        {
            Random rand = new Random();
            
            int length1 = 1;
            float position1 = 0;
            float speed1 = 1f;
            Color cometColor1 = Color.Cyan;

            int length2 = 1;
            float position2 = 50;
            float speed2 = 1.1f;
            Color cometColor2 = Color.Cyan.WithHue(90);

            Color[] colors = new Color[ledStrip.NumberOfLeds];//store a color value for every LED

            //initialize to black
            for (int j = 0; j < ledStrip.NumberOfLeds; j++) { colors[j] = Color.Black; }

            //run the comets
            while (true)
            {
                //move the comets
                position1 += speed1;
                position2 += speed2;

                //bounds check - change direction at the ends of the strip
                if (position1 >= ledStrip.NumberOfLeds - length1 || position1 <= 0) { speed1 *= -1; }
                if (position2 >= ledStrip.NumberOfLeds - length2 || position2 <= 0) { speed2 *= -1; }

                //increment the hue/color
                cometColor1 = cometColor1.WithHue(cometColor1.Hue + 0.0001);
                if (cometColor1.Hue >= 1.0) cometColor1 = cometColor1.WithHue(0);
                cometColor2 = cometColor2.WithHue(cometColor2.Hue + 0.00011); //10% faster than comet 1
                if (cometColor2.Hue >= 1.0) cometColor2 = cometColor2.WithHue(0);

                //update the LEDs 
                for (int j = 0; j < ledStrip.NumberOfLeds; j++)
                {   
                    // draw comets overlapping
                    if (j >= position2 && j < position2 + length2 &&
                        j >= position1 && j < position1 + length1)
                    {   //if both comets are on the same LED - color blend
                        colors[j] = new Color((cometColor1.R + cometColor2.R)/2,
                                              (cometColor1.G + cometColor2.G)/2,
                                              (cometColor1.B + cometColor2.B)/2);
                    }
                    //draw comet 1
                    else if (j >= position1 && j < position1 + length1)
                    {   //set the LED at comet 1's position to its current color
                        colors[j] = cometColor1;
                    }
                    //draw comet 2
                    else if (j >= position2 && j < position2 + length2)
                    {   //set the LED at comet 2's position to its current color
                        colors[j] = cometColor2;
                    }
                    //fade everything else randomly
                    else if (rand.Next() % 3 == 0)
                    {
                        colors[j] = colors[j].WithBrightness(colors[j].Brightness - 0.1);
                    }

                    ledStrip.SetLed(j, colors[j]);
                }

                ledStrip.Show();

                Thread.Sleep(25);
            }
        }

        void Pulse()
        {
            double fadeAmount = 0.5;
            int size = 5;
            byte hueChange = 4;
            byte hue = 0;

            int position = 0;
            int motion = 1;

            while (true)
            {
                ledStrip.Clear();

                position += motion;
                hue += hueChange;

                if(position == ledStrip.NumberOfLeds - size || position == 0)
                {
                    motion *= -1;
                }

                for(int i = 0; i < size; i++)
                {
                    ledStrip.SetLed(position + i, Color.Cyan);
                }

                ledStrip.Show();
            }
        }

   

        void Twinkle()
        {
            var rand = new Random();

            while(true)
            {
                ledStrip.Clear();

                for (int i = 0; i < ledStrip.NumberOfLeds / 4; i++)
                {
                    ledStrip.SetLed(rand.Next() % ledStrip.NumberOfLeds, twinkleColors[rand.Next() % twinkleColors.Length]);
                    ledStrip.Show();

                    Thread.Sleep(250);
                }
            }
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
                    color = Color.Purple;
                }
                else if (offset % 4 == 2)
                {
                    color = Color.Blue;
                }
                else//f (offset % 4 == 1)
                {
                    color = Color.LawnGreen;
                }

                for (int i = 0; i < ledStrip.NumberOfLeds; i++)
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

            while (count > 0)
            {
                for (int i = 0; i < ledStrip.NumberOfLeds; i++)
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

            ShowColorPulse(Color.Silver, duration);
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
            ShowColorPulse(Color.Gold, duration);
        }

        void ShowColorPulse(Color color, int duration = 1000)
        {
            float brightness = 0.05f;
            bool forward = true;

            DateTime time = DateTime.Now;

            while ((DateTime.Now - time).TotalMilliseconds < duration)
            {
                for (int i = 0; i < ledStrip.NumberOfLeds; i++)
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