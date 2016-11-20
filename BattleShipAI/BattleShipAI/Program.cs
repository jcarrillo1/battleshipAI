using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class Program
    {
        static bool PlaceShip(Player player, Ship ship, int x, int y, int direction)
        {
            int x2 = 0;
            int y2 = 0;
            int shipLength = ship.size.Length;
            x--; //Cause it's 0-9 for the board
            y--;

            //Check start location
            if (x < 0 || x > 9 || y < 0 || y > 9 || player.defend[x, y] != 0)
            {
                Console.WriteLine($"Invalid start location.");
                return false;
            }

            switch (direction)
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
                    Console.WriteLine($"Invalid direction.");
                    return false;
            }

            //End location within bounds?
            if (x2 < 0 || y2 < 0 || x2 > 9 || y2 > 9)
            {
                Console.WriteLine($"Start location, direction, and ship size end out of bounds.");
                return false;
            }

            ship.SetLocation(x, y, x2, y2);

            //start x == end x = u/d direction
            if (y2 == y)
            {
                //Down
                if (x2 > x)
                {
                    for (int ix = x; (x2 - ix) > 0; ix++)
                    {
                        if (player.defend[ix, y] != 0)
                        {
                            Console.WriteLine($"Overlapping ships at {ix} {y}");
                            return false;
                        }
                        player.defend[ix, y] = shipLength;
                    }
                }
                //Up
                else
                {
                    for (int ix = x; (x2 - ix) < 0; ix--)
                    {
                        if (player.defend[ix, y] != 0)
                        {
                            Console.WriteLine($"Overlapping ships at {ix} {y}");
                            return false;
                        }
                        player.defend[ix, y] = shipLength;
                    }
                }
            }
            else
            {
                //Right
                if (y2 > y)
                {
                    for (int iy = y; (y2 - iy) > 0; iy++)
                    {
                        if (player.defend[x, iy] != 0)
                        {
                            Console.WriteLine($"Overlapping ships at {x} {iy}");
                            return false;
                        }
                        player.defend[x, iy] = shipLength;
                    }
                }
                //Left
                else
                {
                    for (int iy = y; (y2 - iy) < 0; iy--)
                    {
                        if (player.defend[x, iy] != 0)
                        {
                            Console.WriteLine($"Overlapping ships at {x} {iy}");
                            return false;
                        }
                        player.defend[x, iy] = shipLength;
                    }
                }
            }
            ship.placed = true;
            return true;
        }

        static bool IsShipSunk(Player player, Ship ship)
        {
            if (ship.size.All(x => x == 1))
            {
                ship.sunk = true;
                IsGameOver(player);
                return true;
            }
            return false;
        }

        static bool IsGameOver(Player player)
        {
            if (player.ships.Any(x => x.sunk == false))
            {
                return false;
            }
            Console.WriteLine($"Game over.");
            return true;
        }

        static bool Attack(Player player, Player target, int x, int y)
        {
            x--;
            y--;
            if (player.attack[x, y] != 0)
            {
                Console.WriteLine($"You already shot this location: {x} {y}.");
                return false;
            }
            if (target.defend[x, y] >= 2)
            {
                player.attack[x, y] = 1;

                foreach (Ship shp in target.ships.Where(z => !z.sunk))
                {
                    if (shp.loc.Item1 == shp.loc.Item3 && shp.loc.Item1 == x)
                    {
                        if ((shp.loc.Item2 <= y && y <= shp.loc.Item4) || (shp.loc.Item2 >= y && y >= shp.loc.Item4))
                        {
                            int i = shp.size.First(z => z == 0);
                            shp.size[i] = 1;
                            if (IsShipSunk(player, shp))

                                break;
                        }
                    }
                    else if (shp.loc.Item2 == shp.loc.Item4 && shp.loc.Item2 == y)
                    {
                        if ((shp.loc.Item1 <= x && x <= shp.loc.Item3) || (shp.loc.Item1 >= x && x >= shp.loc.Item3))
                        {
                            int i = shp.size.First(z => z == 0);
                            shp.size[i] = 1;
                            IsShipSunk(player, shp);
                            break;
                        }
                    }
                }
                target.defend[x, y] = 1;
                Console.WriteLine($"It's a hit!");
            }
            else
            {
                player.attack[x, y] = 9;
                target.defend[x, y] = 9;
                Console.WriteLine($"It's a miss.");
            }

            return true;
        }

        static void Main(string[] args)
        {
            Console.WindowWidth = 120;

            Player human1 = new Player();
            Player human2 = new Player(); //TODO: Add computer

            Console.WriteLine($"Player 1 please place your ships.");

            foreach (Ship ship in human1.ships.Where(x => x.placed == false))
            {
                bool placed = false;
                human1.PrintBoard(human1.defend);
                do
                {
                    try
                    {
                        Console.WriteLine($"Please enter a start location in the format x y d for your {ship.name} of size {ship.size.Length}.\nEnter coordinates x y (1-10) and direction d (0 for horizontal, 1 for vertical).");
                        var input = Console.ReadLine().Split(' ');

                        int x = int.Parse(input[0]);
                        int y = int.Parse(input[1]);
                        int dir = int.Parse(input[2]);

                        placed = PlaceShip(human1, ship, x, y, dir);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Invalid Inputs");
                    }

                } while (!placed);
                
            }

            human1.PrintBoard(human1.defend);

            foreach (Ship ship in human2.ships.Where(x => x.placed == false))
            {
                bool placed = false;
                human2.PrintBoard(human2.defend);
                do
                {
                    try
                    {
                        Console.WriteLine($"Please enter a start location, x y (1-10) and direction (0 for horizontal, 1 for vertical) for your {ship.name} of size {ship.size.Length}. Enter as: x y d");
                        var input = Console.ReadLine().Split(' ');

                        int x = int.Parse(input[0]);
                        int y = int.Parse(input[1]);
                        int dir = int.Parse(input[2]);

                        placed = PlaceShip(human2, ship, x, y, dir);
                    } catch (Exception e)
                    {
                        Console.WriteLine("Error");
                    }
                    
                } while (!placed);
                
            }

            human2.PrintBoard(human2.defend);

            while (true)
            {
                Console.WriteLine($"Player 1 please enter your attack as x y, (1-10).");
                var input = Console.ReadLine().Split(' ');
                int x = int.Parse(input[0]);
                int y = int.Parse(input[1]);
                Attack(human1, human2, x, y);

                Console.WriteLine($"Player 2 please enter your attack as x y, (1-10).");
                input = Console.ReadLine().Split(' ');
                x = int.Parse(input[0]);
                y = int.Parse(input[1]);
                Attack(human2, human1, x, y);
                break;
            }
            Console.WriteLine($"1 def");
            human1.PrintBoard(human1.defend);
            Console.WriteLine($"1 att");
            human1.PrintBoard(human1.attack);

            Console.WriteLine($"2 def");
            human2.PrintBoard(human2.defend);
            Console.WriteLine($"2 att");
            human2.PrintBoard(human2.attack);

            Console.ReadKey();
        }

    }
}
