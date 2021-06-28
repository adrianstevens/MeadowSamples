using Meadow.Foundation.Displays.TextDisplayMenu;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Sensors;
using System;
using System.Linq;
using System.Threading;

namespace GymClock
{
    public partial class Clock
    {
        FontBase fontClock;
        FontBase fontDate;

        Menu menu;

        const string ActiveID = "active";
        const string RestId = "rest";
        const string SetTime = "setTime";
        const string SetDate = "setDate";

        ITemperatureSensor temperatureSensor;

        public void Init(GraphicsLibrary gl, ITemperatureSensor tempSensor)
        {
            temperatureSensor = tempSensor;

            fontDate = new Font4x6();
            fontClock = new Font6x8();

            MeadowApp.Device.SetClock(new DateTime(2021, 3, 1));

            InitMenu(gl);

            return; 
        }

        public void Update(GraphicsLibrary gl)
        {
            if (menu != null && menu.IsEnabled)
            {
                return;
            }

            if(state == ClockState.Clock)
            {
                UpdateClock(gl);
            }
            else if(state == ClockState.StopWatch)
            {
                UpdateStopWatch(gl);
            }
            else
            {
                UpdateIntervalTimer(gl);
            }
        }

        string GetStopwatchTime(long ticks)
        {
            ticks /= 1000000;

            long h = ticks / 36000;
            ticks %= 36000;

            long m = ticks / 600;
            ticks %= 600;

            long s = ticks / 10;
            long ms = ticks %= 10;

            return $"{m:00}:{s:00}.{ms:0}";
        }

        void UpdateIntervalTimer(GraphicsLibrary gl)
        {
            gl.Clear();

            long seconds = (DateTime.Now - intervalStart).Ticks / 10000000;
            if(itState == IntervalTimerState.Stop) { seconds = 0; }

            gl.CurrentFont = fontClock;

            gl.DrawText(gl.Width + 1, 0, GetActiveTime(seconds), alignment: GraphicsLibrary.TextAlignment.Right);
            gl.DrawText(gl.Width + 1, 8, GetRestTime(seconds), alignment: GraphicsLibrary.TextAlignment.Right);

            gl.CurrentFont = fontDate;
            gl.DrawText(0, 18, $"{GetInterval(seconds)}");
            gl.DrawText(gl.Width + 1, 18, GetTotalTime(seconds), alignment: GraphicsLibrary.TextAlignment.Right);

            gl.Show();
        }

        int GetInterval(long seconds)
        {
            return (int)(seconds / (interval1 + interval2) + 1);
        }

        string GetTotalTime(long seconds)
        {
            return $"{seconds / 60}:{seconds % 60:00}";
        }

        string GetActiveTime(long seconds)
        {
            int cycleTotal = interval1 + interval2;

            var cycleTime = seconds % cycleTotal;

            int activeTime;

            if(cycleTime >=interval1)
            {
                activeTime = 0;
            }
            else
            {
                activeTime = interval1 - (int)cycleTime;
            }

            return $"{activeTime / 60}:{activeTime % 60:00}";
        }

        string GetRestTime(long seconds)
        {
            int cycleTotal = interval1 + interval2;

            var cycleTime = seconds % cycleTotal;

            int restTime = 0;

            if (cycleTime < interval1)
            {
                restTime = interval2;
            }
            else
            {
                restTime = interval2 - (int)cycleTime + interval1;
            }

            return $"{restTime / 60}:{restTime % 60:00}";
        }

        void UpdateStopWatch(GraphicsLibrary gl)
        {
            gl.Clear();
            gl.CurrentFont = fontDate;

            if(swState == StopWatchState.Start)
            {
                gl.DrawText(0, 0, GetStopwatchTime((DateTime.Now - stopWatchStart).Ticks + stopwatchOffset));
            }
            else
            {
                gl.DrawText(0, 0, GetStopwatchTime(stopwatchOffset));
            }
            
            if(splits.Count > 0)
            {
                //current lap
                if(swState == StopWatchState.Start)
                {
                    gl.DrawText(0, 8, GetStopwatchTime((DateTime.Now - stopWatchStart).Ticks));
                }
                else
                {
                    gl.DrawText(0, 8, GetStopwatchTime(stopwatchOffset - splits.LastOrDefault()));
                }
               
                //last split
                gl.DrawText(0, 16, GetStopwatchTime(splits.LastOrDefault()));

             //   gl.CurrentFont = fontClock;
            //    gl.DrawText(0, 22, $"Lap {splits.Count + 1}");
            //    gl.DrawText(0, 44, $"Lap {splits.Count} split"); 
            }

            gl.Show();
        }

        void UpdateClock(GraphicsLibrary gl)
        {
            gl.Clear();

            gl.CurrentFont = fontClock;
            // gl.DrawText(-1, 0, $"{DateTime.Now.ToString("hh")}");
            var hour = $"{DateTime.Now.ToString("hh").TrimStart('0')}";

            gl.DrawText(11, 0, hour, alignment: GraphicsLibrary.TextAlignment.Right);
            gl.DrawPixel(11, 2);
            gl.DrawPixel(11, 5);
            gl.DrawText(13, 0, $"{DateTime.Now.ToString("mm")}");

            gl.CurrentFont = fontDate;
            var ampm = (DateTime.Now.Hour > 11) ? "PM" : "AM";
            gl.DrawText(25, 2, ampm);

            gl.CurrentFont = fontDate;
            gl.DrawText(16, 10, $"{DateTime.Now.ToString("MMM").ToUpper()} {DateTime.Now.Day}", alignment: GraphicsLibrary.TextAlignment.Center);

            try
            {
                gl.DrawText(16, 18, $"{temperatureSensor?.Temperature.Value.Celsius.ToString("0.0")} C", alignment: GraphicsLibrary.TextAlignment.Center);
            }
            catch
            {

            }

            gl.Show();

            if(state == ClockState.Clock)
            {
                Thread.Sleep(1000);
            }
        }   

        void InitMenu(GraphicsLibrary gl)
        {
            Console.WriteLine("InitMenu");

            var menuItems = new MenuItem[]
            {
                new MenuItem("Clock",
                    subItems: new MenuItem[]{new MenuItem("Date", id: SetDate, type: "Date", value: new DateTime(2021, 2, 20)),
                                             new MenuItem("Time", id: SetTime, type: "Time", value: new TimeSpan(12, 45, 30))
                    }),
                new MenuItem("Interval",
                    subItems: new MenuItem[]{new MenuItem("Active", id: ActiveID, type: "TimeShort", value: new TimeSpan(0, 0, interval1)),
                                             new MenuItem("Rest", id: RestId, type: "TimeShort", value: new TimeSpan(0, 0, interval2)) 
                    })
             };

            menu = new Menu(gl, menuItems);

            menu.ValueChanged += Menu_ValueChanged;

            Console.WriteLine("Menu initialized");
        }

        private void Menu_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            switch (e.ItemID)
            {
                case ActiveID:
                    interval1 = (int)((TimeSpan)e.Value).TotalSeconds;
                    break;
                case RestId:
                    interval2 = (int)((TimeSpan)e.Value).TotalSeconds;
                    break;
                case SetTime:
                    { 
                        var now = DateTime.Now;
                        var time = (TimeSpan)e.Value;
                        MeadowApp.Device.SetClock(new DateTime(now.Year, now.Month, now.Day, time.Hours, time.Minutes, time.Seconds));
                    }
                    break;
                case SetDate:
                    {
                        var now = DateTime.Now;
                        var date = (DateTime)e.Value;
                        MeadowApp.Device.SetClock(new DateTime(date.Year, date.Month, date.Day, now.Hour, now.Minute, now.Second));
                    }
                    break;
            }
        }
    }
}