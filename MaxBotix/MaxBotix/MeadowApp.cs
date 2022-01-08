using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.Threading;

namespace MaxBotix
{
    // Change F7MicroV2 to F7Micro for V1.x boards
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        MaxBotix maxBotix;

        public MeadowApp()
        {
            Initialize();

            maxBotix.LengthUpdated += MaxBotix_LengthUpdated;

            maxBotix.StartUpdating(new TimeSpan(0, 0, 5));
        }

        private void MaxBotix_LengthUpdated(object sender, IChangeResult<Meadow.Units.Length> e)
        {
            Console.WriteLine($"Len: {e.New.Centimeters}");
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            //analog
            //maxBotix = new MaxBotix(Device, Device.Pins.A00, MaxBotix.SensorModel.MB7388);

            //serial
            maxBotix = new MaxBotix(Device, Device.SerialPortNames.Com4, MaxBotix.SensorModel.MB7388);

            Console.WriteLine("Initialized");
        }
    }
}
