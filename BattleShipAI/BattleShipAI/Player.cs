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
        public void PlaceShips()
        {
            
            foreach (Ship ship in ships.Where(x => x.placed == false))
            {
                bool placed = false;
                do
                {
                    int x = StaticRandom.Instance.Next(1, 11);
                    int y = StaticRandom.Instance.Next(1, 11);
                    int dir = StaticRandom.Instance.Next(0, 2);
                    placed = ship.PlaceShip(this, x, y, dir);
                } while (!placed);

            }
        }
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
                        if ((shp.loc.Item2 <= y && y <= shp.loc.Item4) || (shp.loc.Item2 >= y && y >= shp.loc.Item4))
                        {
                            int i = shp.size.First(z => z == 0);
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
                        if ((shp.loc.Item1 <= x && x <= shp.loc.Item3) || (shp.loc.Item1 >= x && x >= shp.loc.Item3))
                        {
                            int i = shp.size.First(z => z == 0);
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
        public bool GenedAttack(Player target, int x, int y)
        {
            
            if (x > 9 || y > 9 || x < 0 || y < 0)
            {
                return false;
            }
            if (attack[x, y] != 0)
            {
                return false;
            }
            if (target.defend[x, y] >= 2)
            {
                attack[x, y] = target.defend[x, y];

                foreach (Ship shp in target.ships.Where(z => !z.sunk))
                {
                    if (shp.loc.Item1 == shp.loc.Item3 && shp.loc.Item1 == x)
                    {
                        if ((shp.loc.Item2 <= y && y <= shp.loc.Item4) || (shp.loc.Item2 >= y && y >= shp.loc.Item4))
                        {
                            int i;
                            for (i = 0; i < shp.size.Length; i++)
                            {
                                if (shp.size[i] == 0) break;
                            }
                            shp.size[i] = 1;
                            if (shp.IsSunk())
                            {
                                Console.WriteLine($"Ship {shp.name} has been sunk");
                            }
                            else
                            {
                                moves.Push(new Point(x + 1, y, shp));
                                moves.Push(new Point(x - 1, y, shp));
                                moves.Push(new Point(x, y - 1, shp));
                                moves.Push(new Point(x, y + 1, shp));
                            }
                            break;
                        }
                    }
                    else if (shp.loc.Item2 == shp.loc.Item4 && shp.loc.Item2 == y)
                    {
                        if ((shp.loc.Item1 <= x && x <= shp.loc.Item3) || (shp.loc.Item1 >= x && x >= shp.loc.Item3))
                        {
                            int i = shp.size.First(z => z == 0);
                            shp.size[i] = 1;
                            if (shp.IsSunk())
                            {
                                Console.WriteLine($"Ship {shp.name} has been sunk");
                            }
                            else
                            {
                                moves.Push(new Point(x + 1, y, shp));
                                moves.Push(new Point(x - 1, y, shp));
                                moves.Push(new Point(x, y - 1, shp));
                                moves.Push(new Point(x, y + 1, shp));
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
        public void GenerateAttack(Player target)
        {
            if (moves.Count > 0)
            {
                
                while (moves.Count > 0)
                {
                    Point nextMove = moves.Pop();
                    bool sunk = false;
                    foreach(Ship ship in target.ships)
                    {
                        if (ship.name == nextMove.ship.name)
                        {
                            if (ship.IsSunk())
                            {
                                sunk = true;
                                break;
                            }
                        }
                    }
                    if (sunk) continue;
                    if (GenedAttack(target, nextMove.x, nextMove.y))
                    {   
                        return;
                    }
                }
            }
            
            if (moves.Count == 0)
            {
                int genX, genY;
                bool attacked = false;
                do
                {
                    genX = StaticRandom.Instance.Next(0, 10);
                    genY = (genX/2) == 0 ? StaticRandom.Instance.Next(0, 17)/2 + 1:StaticRandom.Instance.Next(0,17)/2;
                    attacked = GenedAttack(target, genX, genY);
                } while (!attacked);
            }
            
        }
    }
}
