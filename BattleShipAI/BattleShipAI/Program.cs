using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class Program
    {

        
        
        static bool IsGameOver(Player player)
        {
            if (player.ships.Any(x => x.IsSunk() == false))
            {
                return false;
            }
            //Console.WriteLine($"Game over.");
            return true;
        }

        static void Main(string[] args)
        {
            Console.WindowWidth = 120;
            int gameCount = 0;
            double average = 0;
            double totalGames = 100;
            while (gameCount < totalGames)
            {
                Player human1 = new Player();
                Player human2 = new Player(); //TODO: Add computer

                //Console.WriteLine($"Player 1 please place your ships.");

                //foreach (Ship ship in human1.ships.Where(x => x.placed == false))
                //{
                //    bool placed = false;
                //    human1.PrintBoard(human1.defend);
                //    do
                //    {
                //        try
                //        {
                //            Console.WriteLine($"Please enter a start location in the format x y d for your {ship.name} of size {ship.size.Length}.\nEnter coordinates x y (1-10) and direction d (0 for horizontal, 1 for vertical).");
                //            var input = Console.ReadLine().Split(' ');

                //            int x = int.Parse(input[0]);
                //            int y = int.Parse(input[1]);
                //            int dir = int.Parse(input[2]);

                //            placed = ship.PlaceShip(human1, x, y, dir);
                //        }
                //        catch (Exception e)
                //        {
                //            Console.WriteLine("Invalid Inputs");
                //        }

                //    } while (!placed);

                //}
                human1.PlaceShips();
                //human1.PrintBoard(human1.defend);

                human2.PlaceShips();
                //human2.PrintBoard(human2.defend);
                int moveCount = 0;
                while (true)
                {
                    //bool didAttack = false;
                    //do
                    //{
                    //    try
                    //    {
                    //        Console.WriteLine($"Player 1 please enter your attack as x y, (1-10).");
                    //        var input = Console.ReadLine().Split(' ');
                    //        int x = int.Parse(input[0]);
                    //        int y = int.Parse(input[1]);
                    //        didAttack = human1.Attack(human2, x, y);
                    //    } catch(Exception e)
                    //    {
                    //        Console.WriteLine("Something went wrong, try again");
                    //    }

                    //} while (!didAttack);

                    human1.GenerateAttack(human2);
                    //Console.WriteLine($"1 att");
                    //human1.PrintBoard(human1.attack);
                    if (IsGameOver(human2))
                    {
                        Console.WriteLine($"Move count: {++moveCount}");
                        average += moveCount;
                        //Console.WriteLine($"You won");
                        break;
                    }

                    //human2.GenerateAttack(human1);
                    ////Console.WriteLine($"2 att");
                    ////human1.PrintBoard(human2.attack);
                    //if (IsGameOver(human1))
                    //{
                    //    Console.WriteLine($"Move count: {++moveCount}");
                    //    average += moveCount;
                    //    //Console.WriteLine($"You lost");
                    //    break;
                    //}
                    ++moveCount;
                    //Console.WriteLine($"Move count: {moveCount}");

                    //Console.WriteLine($"2 def");
                    //human1.PrintBoard(human2.defend);

                    //Console.WriteLine($"1 def");
                    //human1.PrintBoard(human1.defend);

                }
                gameCount++;
            }
            average /= totalGames;
            Console.WriteLine($"Average in {totalGames}: {average}");
        }
        

    }
}
