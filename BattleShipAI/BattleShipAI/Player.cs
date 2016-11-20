using BattleShip;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShip
{
    public class Player
    {
        public int[,] defend = new int[10, 10];
        public int[,] attack = new int[10, 10];
        public Stack<Point> moves = new Stack<Point>();
        public List<Ship> ships = new List<Ship>()
        {
            new Ship(2, "destroyer"),
            new Ship(3, "submarine"),
            new Ship(3, "cruiser"),
            new Ship(4, "battleship"),
            new Ship(5, "carrier"),
        };
        public Dictionary<string, int> wildShips = new Dictionary<string, int>()
        {
            { "destroyer", 2 },
            { "submarine", 3 },
            { "cruiser", 3 },
            { "battleship", 4 },
            { "carrier", 5 }
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
        /*
         *  Randomly place ships on your board
         */
        public void PlaceShips()
        {
            
            foreach (Ship ship in ships.Where(x => x.placed == false))
            {
                bool placed = false;
                do
                {
                    int x = StaticRandom.Instance.Next(1, 11);
                    int y = StaticRandom.Instance.Next(1, 11);
                    int dir = StaticRandom.Instance.Next(0, 100) % 2;
                    placed = ship.PlaceShip(this, x, y, dir);
                } while (!placed);

            }
        }
        /*
         * Attack definition for a human
         */
        public bool Attack(Player target, int x, int y)
        {
            x--;
            y--;
            if (x > 9 || y > 9 || x < 0 || y < 0)
            {
                Console.WriteLine($"Out of bounds, choose different points");
                return false;
            }
            if (attack[x, y] != 0)
            {
                Console.WriteLine($"You already shot this location: {x} {y}.");
                return false;
            }
            if (target.defend[x, y] >= 2)
            {
                attack[x, y] = target.defend[x, y];

                foreach (Ship shp in target.ships.Where(z => !z.sunk))
                {
                    if (shp.loc.Item1 == shp.loc.Item3 && shp.loc.Item1 == x)
                    {
                        if (shp.loc.Item2 <= y && y < shp.loc.Item4)
                        {
                            int i;
                            for (i = 0; i < shp.size.Length; i++)
                            {
                                if (shp.size[i] == 0) break;
                            }
                            if (i < shp.size.Length)
                                shp.size[i] = 1;
                            if (shp.IsSunk())
                            {
                                Console.WriteLine($"Ship {shp.name} has been sunk");
                            }
                            else
                            {
                                Console.WriteLine($"Ship {shp.name} has been hit");
                            }
                            break;
                        }
                    }
                    else if (shp.loc.Item2 == shp.loc.Item4 && shp.loc.Item2 == y)
                    {
                        if (shp.loc.Item1 <= x && x < shp.loc.Item3)
                        {
                            int i;
                            for (i = 0; i < shp.size.Length; i++)
                            {
                                if (shp.size[i] == 0) break;
                            }
                            if (i < shp.size.Length)
                                shp.size[i] = 1;
                            if (shp.IsSunk())
                            {
                                Console.WriteLine($"Ship {shp.name} has been sunk");
                            }
                            else
                            {
                                Console.WriteLine($"Ship {shp.name} has been hit");
                            }
                            break;
                        }
                    }
                }
                target.defend[x, y] = 1;
            }
            else
            {
                attack[x, y] = 1;
                target.defend[x, y] = 1;
                Console.WriteLine($"It's a miss.");
            }

            return true;
        }
        /*
         * Check and see if an orientation has been discovered 
         * for a series of hits on a ship
         */
        private void CheckOrientation(int x, int y, Point point)
        {
            int originX = point.originX;
            int originY = point.originY;
            if (Math.Abs(x - originX) > 1)
            {
                int newX = x > originX ? originX - 1 : originX + 1;
                moves.Push(new Point(newX, originY, point.shipName, originX, originY));
            }
            if (Math.Abs(y - originY) > 1)
            {
                int newY = y > originY ? originY - 1 : originY + 1;
                moves.Push(new Point(originX, newY, point.shipName, originX, originY));
            }
        }
        private bool GenedAttack(Player target, int x, int y, Point point)
        {
            
            if (x > 9 || y > 9 || x < 0 || y < 0)
            {
                if (point != null) CheckOrientation(x, y, point);
                return false;
            }
            if (attack[x, y] != 0)
            {
                if (point != null) CheckOrientation(x, y, point);
                return false;
            }
            if (target.defend[x, y] >= 2)
            {
                attack[x, y] = target.defend[x, y];

                foreach (Ship shp in target.ships.Where(z => !z.sunk))
                {
                    if (shp.loc.Item1 == shp.loc.Item3 && shp.loc.Item1 == x)
                    {
                        if (shp.loc.Item2 <= y && y < shp.loc.Item4)
                        {
                            int i;
                            for (i = 0; i < shp.size.Length; i++)
                            {
                                if (shp.size[i] == 0) break;
                            }
                            if (i < shp.size.Length)
                                shp.size[i] = 1;
                            if (shp.IsSunk())
                            {
                                wildShips.Remove(shp.name);
                                //Console.WriteLine($"Ship {shp.name} has been sunk");
                            }
                            else if (point != null && shp.name == point.shipName)
                            {
                                int originX = point.originX;
                                int originY = point.originY;
                                int newX = x == originX ? x : originX > x ? x - 1 : x + 1;
                                int newY = y == originY ? y : originY > y ? y - 1 : y + 1;
                                moves.Push(new Point(newX, newY, point.shipName, originX, originY));
                            }
                            else
                            {
                                if (point != null) CheckOrientation(x, y, point);
                                moves.Push(new Point(x + 1, y, shp.name, x, y));
                                moves.Push(new Point(x, y - 1, shp.name, x, y));
                                moves.Push(new Point(x - 1, y, shp.name, x, y));
                                moves.Push(new Point(x, y + 1, shp.name, x, y));
                            }
                            break;
                        }
                    }
                    else if (shp.loc.Item2 == shp.loc.Item4 && shp.loc.Item2 == y)
                    {
                        if (shp.loc.Item1 <= x && x < shp.loc.Item3)
                        {
                            int i;
                            for (i = 0; i < shp.size.Length; i++)
                            {
                                if (shp.size[i] == 0) break;
                            }
                            if (i < shp.size.Length)
                                shp.size[i] = 1;
                            if (shp.IsSunk())
                            {
                                wildShips.Remove(shp.name);
                                //Console.WriteLine($"Ship {shp.name} has been sunk");
                            }
                            else if (point != null && shp.name == point.shipName)
                            {
                                int originX = point.originX;
                                int originY = point.originY;
                                int newX = x == originX ? x : originX > x ? x - 1 : x + 1;
                                int newY = y == originY ? y : originY > y ? y - 1 : y + 1;
                                moves.Push(new Point(newX, newY, point.shipName, originX, originY));
                            }
                            else
                            {
                                if (point != null) CheckOrientation(x, y, point);
                                moves.Push(new Point(x + 1, y, shp.name, x, y));
                                moves.Push(new Point(x, y - 1, shp.name, x, y));
                                moves.Push(new Point(x - 1, y, shp.name, x, y));
                                moves.Push(new Point(x, y + 1, shp.name, x, y));
                            }
                            break;
                        }
                    }
                }
                target.defend[x, y] = 1;
            }
            else
            {
                // check if previously hit ship has an orientation figured out
                if (point != null) CheckOrientation(x, y, point);
                attack[x, y] = 1;
                target.defend[x, y] = 1;
                //Console.WriteLine($"It's a miss.");
            }

            return true;
        }
        public void GenerateAttack(Player target)
        {
            if (moves.Count > 0)
            {
                
                while (moves.Count > 0)
                {
                    Point nextMove = moves.Pop();
                    Ship ship = target.ships.Find(x => nextMove.shipName == x.name);
                    if (ship.sunk) continue;
                    
                    if (GenedAttack(target, nextMove.x, nextMove.y, nextMove))
                    {   
                        return;
                    }
                }
            }
            
            if (moves.Count == 0)
            {
                //int genX, genY;
                bool attacked = false;
                do
                {
                    // Checkerboard generations
                    //genX = StaticRandom.Instance.Next(0, 10);
                    //genY = StaticRandom.Instance.Next(0, 9);
                    //if ((genX % 2 == 0 && genY % 2 != 1) || (genX % 2 == 1 && genY % 2 != 0))
                    //{
                    //    genY += 1;
                    //}
                    var bestMove = GenerateHit();
                    // Random generations
                    //genX = StaticRandom.Instance.Next(0, 10);
                    //genY = StaticRandom.Instance.Next(0, 10);
                    attacked = GenedAttack(target, bestMove.Item1, bestMove.Item2, null);
                } while (!attacked);
            }
            
        }
        private Tuple<int, int> GenerateHit()
        {
            
            int[,] probability = new int[10, 10];
            foreach (KeyValuePair<string, int> ship in wildShips)
            {
                int shipSize = ship.Value;
                for (int row = 0; row < 10; row++)
                {
                    int streak = 0;
                    for (int col = 0; col < 10; col++)
                    {
                        if (attack[row, col] != 0)
                        {
                            streak = 0;
                            continue;
                        }
                        else if (attack[row, col] == 0 && streak < shipSize) streak++;
                        if (streak == shipSize)
                        {
                            for (int i = col - shipSize + 1; i <= col; i++)
                            {
                                probability[row, i] += 1;
                            }
                        }
                    }
                    streak = 0;
                    for (int col = 0; col < 10; col++)
                    {
                        if (attack[col, row] != 0)
                        {
                            streak = 0;
                            continue;
                        }
                        else if (attack[col, row] == 0 && streak < shipSize) streak++;
                        if (streak == shipSize)
                        {
                            for (int i = col - shipSize + 1; i <= col; i++)
                            {
                                probability[i, row] += 1;
                            }
                        }
                    }
                }
            }
            int x = 0, y = 0, maxProb = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (probability[i, j] > maxProb)
                    {
                        maxProb = probability[i, j];
                        x = i;
                        y = j;
                    }
                }
            }
            return new Tuple<int,int>(x, y);
            
        }
    }
}
