using System;
using System.Collections.Generic;

namespace GymClock
{
    public partial class Clock
    {
        public enum ClockState
        {
            Clock,
            StopWatch,
            IntervalTimer,
        }

        enum StopWatchState
        {
            Stop,
            Start, 
            Pause
        }

        enum IntervalTimerState
        {
            Stop,
            Start
        }

        ClockState state = ClockState.Clock;
        StopWatchState swState = StopWatchState.Stop;
        IntervalTimerState itState = IntervalTimerState.Stop;

        DateTime stopWatchStart = DateTime.MinValue;
        DateTime intervalStart = DateTime.MinValue;

        List<long> splits = new List<long>();
        long stopwatchOffset;
        long lastSplit;

        int interval1 = 180;
        int interval2 = 60;

        public void Up()
        {
            if (menu.IsEnabled)
            {
                menu.Previous();
                return;
            }

            if(state == ClockState.StopWatch)
            {
                if (swState == StopWatchState.Stop)
                {
                }
                else if (swState == StopWatchState.Start)
                {   //split
                    stopwatchOffset += DateTime.Now.Ticks - stopWatchStart.Ticks;
                    stopWatchStart = DateTime.Now;
                    splits.Add(stopwatchOffset - lastSplit);
                    lastSplit = stopwatchOffset;
                }
                else //we're paused so stop
                {
                    //reset 
                    splits = new List<long>();
                    stopwatchOffset = 0;
                    lastSplit = 0;
                    swState = StopWatchState.Stop;
                }
            }
            else if(state == ClockState.IntervalTimer)
            {
                if(itState == IntervalTimerState.Start)
                {
                    itState = IntervalTimerState.Stop;
                }
            }
        }

        public void Down()
        {
            if (menu.IsEnabled)
            {
                menu.Next();
                return;
            }

            if (state == ClockState.StopWatch)
            {
                if(swState == StopWatchState.Stop)
                {
                    stopwatchOffset = 0;
                    lastSplit = 0;
                    stopWatchStart = DateTime.Now;
                    swState = StopWatchState.Start;
                }
                else if(swState == StopWatchState.Start)
                {   //pause
                    swState = StopWatchState.Pause;
                    stopwatchOffset += DateTime.Now.Ticks - stopWatchStart.Ticks;
                }
                else //we're paused so resume
                {
                    stopWatchStart = DateTime.Now;
                    swState = StopWatchState.Start;
                }
            }
            else if (state == ClockState.IntervalTimer)
            {
                if (itState == IntervalTimerState.Stop)
                {
                    itState = IntervalTimerState.Start;
                    intervalStart = DateTime.Now;
                }
            }
        }

        public void Left()
        {
            if(menu.IsEnabled == false)
            {
                menu.Enable();
            }
            else if(menu.IsEnabled)
            {
                menu.Back();
            }
        }

        public void Reset()
        {
         
        }

        public void Right()
        {
            if (menu.IsEnabled)
            {
                menu.Select();
                return;
            }

            switch (state)
            {
                case ClockState.Clock:
                    state = ClockState.StopWatch;
                    break;
                case ClockState.StopWatch:
                    state = ClockState.IntervalTimer;
                    break;
                default:
                    state = ClockState.Clock;
                    break;
            }
        }
    }
}