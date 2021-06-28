using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Temperature;

namespace GymClock
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        GraphicsLibrary gl;
        PushButton next, back, select, menu;
        PiezoSpeaker buzzer;

        Tmp102 tempSensor;

        Clock clock;

        public MeadowApp()
        {
            Initialize();

            clock = new Clock();
            clock.Init(gl, tempSensor);

            gl.Clear();
            gl.DrawLine(0, 0, 4, 4, true);
            gl.CurrentFont = new Font4x6();
            gl.DrawText(0, 0, "yoo");
            gl.Show();

            _ = UpdateClock();

            Task.Run(() =>
            {
                while(true)
                {
                    clock.Update(gl);

                    Thread.Sleep(10);
                }
            });
        }

        async Task UpdateClock()
        {
            await Device.InitWiFiAdapter();

            var result = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);

            var today = await ClockService.GetTime();

            MeadowApp.Device.SetClock(today.Subtract(new TimeSpan(7, 0, 0)));
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            var display = new Max7219(Device,
                Device.CreateSpiBus(),
                Device.Pins.D00,
                4, 3,
                Max7219.Max7219Type.Display);

            tempSensor = new Tmp102(Device.CreateI2cBus());
            tempSensor.StartUpdating(new TimeSpan(0, 5, 0));

            display.IgnoreOutOfBoundsPixels = true;

            display.SetBrightness(0);
            display.SetBrightness(6, 0);
            display.SetBrightness(6, 1);
            display.SetBrightness(6, 2);
            display.SetBrightness(6, 3);

            gl = new GraphicsLibrary(display);
            gl.Rotation = GraphicsLibrary.RotationType._90Degrees;
            
            gl.Clear();

            gl.Show();

            next = new PushButton(Device, Device.Pins.D05, Meadow.Hardware.ResistorMode.InternalPullDown);
            back = new PushButton(Device, Device.Pins.D04, Meadow.Hardware.ResistorMode.InternalPullDown);
            select = new PushButton(Device, Device.Pins.D03, Meadow.Hardware.ResistorMode.InternalPullDown);

            next.Clicked += Next_Clicked;
            back.Clicked += Back_Clicked;

            select.Clicked += Select_Clicked;
            select.LongClicked += Select_LongClicked;


            buzzer = new PiezoSpeaker(Device, Device.Pins.D02);
        }

        private void Select_LongClicked(object sender, EventArgs e)
        {
            clock.Left();
        }

        private void Select_Clicked(object sender, EventArgs e)
        {
            clock.Right();
        }

        private void Back_Clicked(object sender, EventArgs e)
        {
            clock.Up();
        }

        private void Next_Clicked(object sender, EventArgs e)
        {
            clock.Down();
        }
    }
}