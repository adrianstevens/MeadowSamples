using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    class Connect4Game
    {
        public byte Width => 7;
        public byte Height => 6;

        public byte[,] GameField { get; private set; }

        public GameStateType GameState { get; private set; };

        public enum GameStateType
        {
            Player1Turn,
            PLayer2Turn,
            Player1Wins,
            Player2Wins,
            Draw,
        };

        public Connect4Game()
        {
            Reset();
        }

        public bool AddChip(byte column)
        {


            for(int i = 0; i < Height; i++)
            {
                if(GameField[i, column] != 0) { continue; }
                {
                    GameField[i, column] = (byte)((GameState == GameStateType.Player1Turn)?1:2);
                    CheckGameState();
                    return true;
                }
            }

            return false;
        }

        public void Reset ()
        {
            GameField = new byte[Width, Height];
            GameState = GameStateType.Player1Turn;
        }

        void CheckGameState()
        {

        }


    }
}
