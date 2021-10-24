using Meadow.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfficeLights
{
    class BouncingBallEffect
    {
        Color[] colorArray =
{
            Color.Green,
            Color.Red,
            Color.Blue,
            Color.Orange,
            Color.Indigo
        };

        int Length { get; set; }
        int BallCount { get; set; }
        double FadeRate { get; set; }
        bool IsMirrored { get; set; }

        const double Gravity = -9.81;
        const double StartHeight = 1.0;
        double ImpactVelocity => InitialBallSpeed(StartHeight);
        const double SpeedKnob = 4.0;

        double[] Height;
        double[] BallSpeed;
        double[] Dampening;
        double[] ClockTimeAtLastBounce;
        Color[] Colors;

        public BouncingBallEffect(int length, int ballCount = 3, double fade = 0, bool isMirrored = false)
        {
            Length = length;
            ballCount = BallCount;
            FadeRate = fade;
            IsMirrored = isMirrored;

            Height = new double[ballCount];
            Dampening = new double[ballCount];
            ClockTimeAtLastBounce = new double[ballCount];
            BallSpeed = new double[ballCount];
            Colors = new Color[ballCount];

            for (int i = 0; i < ballCount; i++)
            {
                Height[i] = StartHeight;
                ClockTimeAtLastBounce[i] = Time();
                Dampening[i] = 0.9 - i / Math.Pow(ballCount, 2);
                BallSpeed[i] = InitialBallSpeed(Height[i]);
                Colors[i] = Colors[i % colorArray.Length];
            }
        }

        double InitialBallSpeed(double height)
        {
            return Math.Sqrt(-2 * Gravity * height);
        }

        double Time()
        {
            return DateTime.Now.Ticks / 1000000.0;// + DateTime.Now.Second;
        }
    }
}
