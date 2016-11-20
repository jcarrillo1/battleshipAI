using BattleShip;
using System;
using System.Collections.Generic;

namespace BattleShip
{
    class Player
    {
        public int[,] defend = new int[10, 10];
        public int[,] attack = new int[10, 10];
        public List<Ship> ships = new List<Ship>()
        {
            new Ship(2, "destroyer"),
            new Ship(3, "submarine"),
            new Ship(3, "cruiser"),
            new Ship(4, "battleship"),
            new Ship(5, "carrier"),
        };

        public void PrintBoard(int[,] board)
        {
            Console.Write($"   ");
            for (int i = 0; i < board.GetLength(0); i++)
            {
                Console.Write($"{i + 1} ");
            }
            Console.WriteLine();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                if (i != 9) Console.Write($"{i + 1}  ");
                else Console.Write($"{i + 1} ");
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write($"{board[i, j]} ");
                }
                Console.WriteLine();
            }
        }
    }
}
