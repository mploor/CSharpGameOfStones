using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameOfStones
{
    public class Stones
    {
        public int[] Piles;

        public Stones()
        {
            Piles = new int[] { 5, 5, 5 };
        }

        public void PileInit()
        {
            int[] Piles = new int[] { 5, 5, 5 };
            //return Piles;
        }

        // Draw one pile of stones
        private static void DrawPile(string pileName, int pileCount)
        {
            Console.Write("{0}: ", pileName);
            for (int i = 0; i < pileCount; i++)
            {
                Console.Write("O");
            }
            Console.WriteLine();
        }

        // Draw all piles of stones
        public void DrawPiles()
        {
            Console.Clear();
            DrawPile("A", Piles[0]);
            DrawPile("B", Piles[1]);
            DrawPile("C", Piles[2]);
            Console.WriteLine();
        }

        // Check for winning move, return null if none, pile number if there is a winning move
        public int? WinMove()
        {
            int remainingPiles = 0;
            int lastPile = 0;
            for (int i = 0; i < Piles.Count(); i++)
            {
                if (Piles[i] > 0)
                {
                    remainingPiles++;
                    lastPile = i;
                }
            }
            if (remainingPiles == 1)
            {
                return lastPile;
            } else
            {
                return null;
            }
        }

        // Determine computer move
        public string CompMove(Random rnd)
        {
            bool goodMove = false;
            int pileChoice = 0;
            int countChoice = 0;
            string[] piles = new string[] { "A", "B", "C" };

            if (WinMove() != null)
            {
                return piles[(int)WinMove()] + Piles[(int)WinMove()].ToString();
            }

            while (!goodMove)
            {  
                pileChoice = rnd.Next(3);
                if (Piles[pileChoice] > 0)
                {
                    countChoice = rnd.Next(Piles[pileChoice]) + 1;
                    goodMove = true;
                } 
            }
            return piles[pileChoice] + countChoice.ToString();
        }

        // Returns true if game has been won (all piles empty)
        public bool Winner()
        {
            if (Piles[0] == 0 && Piles[1] == 0 && Piles[2] == 0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        // Remove specified stones from specified pile
        public int[] DoMove(string pile, int num)
        {
            int pileIndex = 0;
            switch (pile)
            {
                case "A":
                    pileIndex = 0;
                    break;
                case "B":
                    pileIndex = 1;
                    break;
                case "C":
                    pileIndex = 2;
                    break;
                default:
                    break;
            }
            Piles[pileIndex] -= num;
            return Piles;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            bool badMove = false;
            string userMove;
            string compMove;
            string pile;
            int num;
            bool gameOver = false;
            Random rnd = new Random();
            Stones myStones = new Stones();

            myStones.DrawPiles();

            while (!gameOver)
            {
                // Get and validate user move
                do
                {
                    badMove = false;
                    Console.WriteLine("Enter move, letter and number for pile and number of stones to remove");
                    userMove = Console.ReadLine();

                    pile = userMove.Substring(0, 1).ToUpper();
                    if (!(pile == "A" || pile == "B" || pile == "C"))
                    {
                        Console.WriteLine("Invalid entry - pile letter must be a, b, or c");
                        badMove = true;
                    }
                    bool goodNum = int.TryParse(userMove.Substring(1, 1), out num);
                    if (!goodNum)
                    {
                        Console.WriteLine("Invalid entry - must include integer number");
                        badMove = true;
                    }
                } while (badMove);

                myStones.DoMove(pile, num);
                myStones.DrawPiles();

                // Check if player won
                if (myStones.Winner())
                {
                    Console.WriteLine("You Win!");
                    gameOver = true;
                }

                // Computer move
                if (!gameOver)
                {
                    Thread.Sleep(1200);
                    compMove = myStones.CompMove(rnd);
                    myStones.DoMove(compMove[0].ToString(), int.Parse(compMove[1].ToString()));
                    myStones.DrawPiles();
                    Console.WriteLine("Computer move: {0}", compMove);

                    // Check if computer won
                    if (myStones.Winner())
                    {
                        Console.WriteLine("I Win!");
                        gameOver = true;
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
