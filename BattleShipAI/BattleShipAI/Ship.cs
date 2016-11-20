using BattleShip;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShip
{
    public class Ship
    {
        public int[] size;
        public Tuple<int, int, int, int> loc;
        public string name;
        public bool sunk = false;
        public bool placed = false;

        public Ship(int sz, string nme)
        {
            size = new int[sz];
            name = nme;
        }
        public void SetLocation(int x, int y, int x2, int y2)
        {
            loc = new Tuple<int, int, int, int>(x, y, x2, y2);
        }
        public bool IsSunk()
        {
            if (size.All(x => x == 1))
            {
                sunk = true;
                return true;
            }
            return false;
        }
        public bool PlaceShip(Player player, int x, int y, int d)
        {
            int x2 = 0;
            int y2 = 0;
            int shipLength = size.Length;
            x--; //Cause it's 0-9 for the board
            y--;

            //Check start location
            if (x < 0 || x > 9 || y < 0 || y > 9 || player.defend[x, y] != 0)
            {
                //Console.WriteLine($"Invalid start location.");
                return false;
            }

            switch (d)
            {
                // Horizontal
                case 1:
                    x2 = x + shipLength;
                    y2 = y;
                    break;
                // Vertical down
                case 0:
                    x2 = x;
                    y2 = y + shipLength;
                    break;
                default:
                    //Console.WriteLine($"Invalid direction.");
                    return false;
            }

            //End location within bounds?
            if (x2 < 0 || y2 < 0 || x2 > 9 || y2 > 9)
            {
                //Console.WriteLine($"Start location, direction, and ship size end out of bounds.");
                return false;
            }

            SetLocation(x, y, x2, y2);
            if (x2 > x)
            {
                for (int row = x; row < x2; row++)
                {
                    if (player.defend[row, y] != 0)
                    {
                        //Console.WriteLine($"Overlapping ships at {row} {y}");
                        return false;
                    }
                }
                for (int row = x; row < x2; row++)
                {
                    player.defend[row, y] = shipLength;
                }
            }
            else
            {
                for (int col = y; col < y2; col++)
                {
                    if (player.defend[x, col] != 0)
                    {
                        //Console.WriteLine($"Overlapping ships at {x} {col}");
                        return false;
                    }
                }
                for(int col = y; col < y2; col++)
                {
                    player.defend[x, col] = shipLength;
                }
            }
            ////start x == end x = u/d direction
            //if (y2 == y)
            //{
            //    //Down
            //    if (x2 > x)
            //    {
            //        for (int ix = x; (x2 - ix) > 0; ix++)
            //        {
            //            if (player.defend[ix, y] != 0)
            //            {
            //                Console.WriteLine($"Overlapping ships at {ix} {y}");
            //                return false;
            //            }
            //            player.defend[ix, y] = shipLength;
            //        }
            //    }
            //    //Up
            //    else
            //    {
            //        for (int ix = x; (x2 - ix) < 0; ix--)
            //        {
            //            if (player.defend[ix, y] != 0)
            //            {
            //                Console.WriteLine($"Overlapping ships at {ix} {y}");
            //                return false;
            //            }
            //            player.defend[ix, y] = shipLength;
            //        }
            //    }
            //}
            //else
            //{
            //    //Right
            //    if (y2 > y)
            //    {
            //        for (int iy = y; (y2 - iy) > 0; iy++)
            //        {
            //            if (player.defend[x, iy] != 0)
            //            {
            //                Console.WriteLine($"Overlapping ships at {x} {iy}");
            //                return false;
            //            }
            //            player.defend[x, iy] = shipLength;
            //        }
            //    }
            //    //Left
            //    else
            //    {
            //        for (int iy = y; (y2 - iy) < 0; iy--)
            //        {
            //            if (player.defend[x, iy] != 0)
            //            {
            //                Console.WriteLine($"Overlapping ships at {x} {iy}");
            //                return false;
            //            }
            //            player.defend[x, iy] = shipLength;
            //        }
            //    }
            //}
            placed = true;
            return true;
        }
    }
}
