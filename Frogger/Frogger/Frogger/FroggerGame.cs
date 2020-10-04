using System;

namespace Frogger
{
    public partial class FroggerGame
    {
        //each lane has a velocity (float)
        //each lane shows one obstical type
        //safe or not safe
        public enum Direction : byte
        {
            Left,
            Up,
            Right,
            Down,
        }

        public float[] LaneSpeeds { get; private set; } = new float[6] { 1.0f, -1.6f, 1.2f, -1.3f, 1.5f, -1.0f };
        public byte[,] LaneData { get; private set; } = new byte[6, 32]
        {
            //docks
            {1,2,3,0,1,2,3,0,0,0,1,2,3,0,1,2,3,0,0,0,1,2,3,0,0,0,0,1,2,3,0,0 },//logs
            {0,0,1,3,0,0,0,1,2,3,0,0,1,3,0,0,1,3,0,0,0,1,3,0,1,3,0,0,1,2,3,0 },//logs
            {1,2,3,0,1,2,3,0,0,0,1,2,3,0,1,2,3,0,0,0,1,2,2,3,0,0,0,1,2,3,0,0 },//logs
            {0,0,1,3,0,1,3,0,0,0,0,0,0,0,0,0,1,3,0,0,0,0,0,0,1,3,0,0,1,3,0,0 },//trucks
            {0,0,1,2,0,0,0,0,0,0,0,0,1,2,0,0,0,0,0,0,0,1,2,0,1,2,0,0,0,0,0,0 },//cars
            {1,2,3,0,0,0,0,0,0,0,0,1,2,3,0,0,0,0,0,1,2,2,3,0,0,0,0,0,0,0,0,0 },//trucks
            //start
        };

        public double GameTime { get; private set; }

        public byte LaneLength => 32;
        public byte Columns => 16;
        public byte Rows => 8;
        public byte CellSize { get; private set; }

        public int FrogX { get; private set; }
        public int FrogY { get; private set; }

        public byte Lives { get; private set; }

        public Direction LastDirection { get; private set; }

        public FroggerGame(byte cellSize = 8)
        {
            CellSize = cellSize;

            Reset();
        }

        void Reset()
        {
            ResetLevel();
            Lives = 3;
        }

        void ResetLevel()
        {
            gameStart = DateTime.Now;
            FrogX = (Columns / 2) * CellSize;
            FrogY = (Rows - 1) * CellSize;
        }

        DateTime gameStart;
        DateTime lastUpdate;
        DateTime lastMove;
        public void Update()
        {
            var lane = GetFrogLane();
            Console.WriteLine($"lane: {lane}");

            if(lane < 3)
            {
                FrogX -= (int)(LaneSpeeds[lane] * CellSize * (DateTime.Now - lastUpdate).TotalSeconds); 
            }

            lastUpdate = DateTime.Now;
            GameTime = (lastUpdate - gameStart).TotalSeconds;

            CheckCollisions();
        }

        int GetLaneOffset(byte lane, double time)
        {
            var offset = (int)(time * LaneSpeeds[lane]) % LaneLength;

            if (offset < 0)
            {
                offset = LaneLength - (Math.Abs(offset) % 64);
            }

            return offset;
        }

        public int GetLaneOffset(byte lane)
        {
            return GetLaneOffset(lane, GameTime);
        }

        public int GetCellOffset(byte lane, byte cellSize)
        {
            return (int)(cellSize * GameTime * LaneSpeeds[lane]) % cellSize;
        }

        public void OnUp()
        {
            if(FrogY >= CellSize)
            {
                FrogY -= CellSize;
                LastDirection = Direction.Up;
                lastMove = DateTime.Now;
            }
        }

        public void OnDown()
        {
            if (FrogY < (Rows - 1) * CellSize)
            {
                FrogY += CellSize;
                LastDirection = Direction.Up;//for now
                lastMove = DateTime.Now;
            }
        }

        public void OnLeft()
        {
            if (FrogX >= CellSize)
            {
                FrogX -= CellSize;
                LastDirection = Direction.Left;
                lastMove = DateTime.Now;
            }
        }

        public void OnRight()
        {
            if (FrogY < (Columns - 1) * CellSize)
            {
                FrogX += CellSize;
                LastDirection = Direction.Right;
                lastMove = DateTime.Now;
            }
        }

        byte GetFrogLane()
        {
          //  Console.WriteLine($"FrogY {FrogY}, CellSize {CellSize}");

            return (byte)(FrogY / CellSize - 1);
        }

        void CheckCollisions ()
        {
            //check edges
            if(FrogX < 0 || FrogX + CellSize > CellSize * Columns)
            {
                KillFrog();
            }

            var lane = GetFrogLane();
            //check traffic
            if(lane > 2 && lane < 6)
            {

            }
        }

        void KillFrog()
        {
            if(Lives > 1)
            {
                Lives--;
            }
            ResetLevel();
        }
    }
}