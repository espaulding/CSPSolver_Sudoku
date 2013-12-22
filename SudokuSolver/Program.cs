/*
 * Eric Spaulding
 * Professor Alden Wright
 * AI - Fall2012
 */

using System;
using SudokuSolver.Problem;
using System.Diagnostics;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowLeft = 0;
            Console.WindowTop = 0;
            Console.WindowHeight = Console.LargestWindowHeight - 10;
            //Console.WindowWidth = Console.LargestWindowWidth - 5;

            string filename = " ";
            if (args.Length == 0)
            {
                Console.WriteLine("Error: the file was not found.");
                Console.ReadLine(); //pause to display output
                System.Environment.Exit(1);
            }
            else
            {
                //It would be ideal to do some form of security or untainting of the filename
                //before the program dares to use it.
                filename = args[0];
            }

            string error;
            CSP prob = FileIO.ReadProblem(filename, out error);
            if (error.Length > 0)
            {
                Console.WriteLine("Error: " + error);
                Console.ReadLine(); //pause to display output
                System.Environment.Exit(1);
            }

            Console.WriteLine("Input was...\n");
            Console.WriteLine(prob.current.ToString());

            Console.WriteLine("\n\nsolving puzzle...");

            Stopwatch sw = Stopwatch.StartNew();
            //Algo.mode = Algo.Mode.verbose;
            if (prob.Solve()) { Console.WriteLine("A possible solution is"); }
            else { Console.WriteLine("The puzzle is unsolvable"); }

            //double time = 0;
            //for (int i = 0; i < 2000; i++)
            //{
            //    prob = FileIO.ReadProblem(filename, out error);
            //    Stopwatch sw = Stopwatch.StartNew();
            //    prob.Solve();
            //    time += sw.Elapsed.TotalMilliseconds;
            //}
            //time /= 2000;
            //Console.WriteLine("in {0} ms on average over 2000 trials", time);

            Console.WriteLine(prob.current.ToString());
            Console.WriteLine("in {0} ms", sw.Elapsed.TotalMilliseconds);
            Console.WriteLine("in {0} seconds", sw.Elapsed.TotalSeconds);
            Console.WriteLine("in {0} min", sw.Elapsed.TotalMinutes);
            Console.ReadLine(); //pause to display output
        }
    }
}
