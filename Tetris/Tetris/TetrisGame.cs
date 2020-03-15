using System;

namespace Tetris
{
    public class TetrisGame
    {
        public class Tetramino
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Rotation { get; set; }
            public int PieceType { get; set; }

            public void Rotate()
            {
                Rotation = (Rotation += 1) % 4;
            }
        }
        
        public Tetramino CurrentPiece { get; protected set; }

        public int Score { get; private set; }
        public int Level { get; private set; }
        public int LinesCleared { get; private set; }

        public int Width { get; private set; } = 8;
        public int Height { get; private set; } = 32;

        public byte[,] GameField { get; private set; }

        Random rand;

        private byte[][] Tetraminos =
        {
            new byte[] { 0,0,1,0,
                         0,1,1,0,
                         0,0,1,0,
                         0,0,0,0},
            new byte[] { 0,0,1,0,
                         0,0,1,0,
                         0,0,1,0,
                         0,0,1,0},
            new byte[] { 0,0,0,0,
                         0,1,1,0,
                         0,1,1,0,
                         0,0,0,0},
            new byte[] { 0,1,1,0,
                         0,0,1,0,
                         0,0,1,0,
                         0,0,0,0},
            new byte[] { 0,1,1,0,
                         0,1,0,0,
                         0,1,0,0,
                         0,0,0,0},
            new byte[] { 0,1,0,0,
                         0,1,1,0,
                         0,0,1,0,
                         0,0,0,0},
            new byte[] { 0,0,1,0,
                         0,1,1,0,
                         0,1,0,0,
                         0,0,0,0},
        };

        public TetrisGame()
        {
            Init();
            Reset();
        }

        void Init()
        {
            GameField = new byte[Width, Height];
            rand = new Random();
        }

        public void Reset ()
        {
            Score = 0;
            Level = 0;
            LinesCleared = 0;

            for(int x = 0; x < GameField.GetLength(0); x++)
            {
                for(int y = 0; y < GameField.GetLength(1); y++)
                {
                    GameField[x, y] = 0;
                }
            }

            CurrentPiece = GetNewPiece();
        }

        Tetramino GetNewPiece()
        {
            int index = rand.Next(6);

            return new Tetramino()
            {
                X = 2,
                Y = 0,
                Rotation = 0,
                PieceType = index,
            };
        }

        public void OnLeft()
        {
            if(CheckCollision(CurrentPiece.X - 1, CurrentPiece.Y, 
                CurrentPiece.Rotation, Tetraminos[CurrentPiece.PieceType]) == false)
            {
                CurrentPiece.X += -1;
            }
        }

        public void OnRight()
        {
            if (CheckCollision(CurrentPiece.X + 1, CurrentPiece.Y,
                            CurrentPiece.Rotation, Tetraminos[CurrentPiece.PieceType]) == false)
            {
                CurrentPiece.X += 1;
            }
        }

        public void OnRotate()
        {
            if (CheckCollision(CurrentPiece.X, CurrentPiece.Y,
                                CurrentPiece.Rotation + 1, Tetraminos[CurrentPiece.PieceType]) == false)
            {
                CurrentPiece.Rotation = (CurrentPiece.Rotation + 1 ) % 4;
            }
        }

        public void OnDown()
        {
            if (CheckCollision(CurrentPiece.X, CurrentPiece.Y + 1,
                                CurrentPiece.Rotation, Tetraminos[CurrentPiece.PieceType]) == false)
            {
                CurrentPiece.Y += 1;
            }
        }

        bool CheckCollision(int x, int y, int rotation, byte[] pieceData)
        {
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if(IsGameFieldFree(x + i, y + j) == false && 
                        IsLocationSet(i, j, rotation, pieceData))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsGameFieldFree(int x, int y)
        {
            if(x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return false;
            }

            return GameField[x, y] == 0;
        }

        public bool IsLocationSet(int x, int y, int rotation, byte[] pieceData)
        {
            if (x < 0 || x > 3 || y < 0 || y > 3)
            {
                return false;
            }

            switch (rotation % 4)
            {
                case 0:
                    return pieceData[y * 4 + x] == 1;
                case 1:
                    return pieceData[12 + y - (x * 4)] == 1;
                case 2:
                    return pieceData[15 - (y * 4) - x] == 1;
                case 3:
                default:
                    return pieceData[3 + y + (x * 4)] == 1;
            }
        }
    }
}
