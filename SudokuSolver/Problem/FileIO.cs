/*
 * Eric Spaulding
 * Professor Alden Wright
 * AI - Fall2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SudokuSolver.Problem
{
    static class FileIO
    {
        static public CSP ReadProblem(string filename, out string error)
        {
            error = "";
            string line;
            string[] data = { "" };
            byte x = 0, y = 0;
            State start = new State(9,9,9);

            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Replace(" ","").Replace("\t","").Length != 0)
                        {
                            x = 0;
                            data = line.Split(' ');
                            foreach (string s in data)
                            {
                                if (s.Length > 0)
                                {
                                    int value;
                                    int.TryParse(s, out value);
                                    if (value != 0)
                                    {
                                        start.board[x,y].value = value;
                                        start.board[x,y].EmptyDomain();
                                        start.board[x,y].AddToDomain(value);
                                    }
                                    x++;
                                }
                            }
                            y++;
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                error = "the file was not found.";
            }
            catch (Exception)
            {
                error = "the game file was unreadable.";
            }
            return new Sudoku(start,9,9,9);
        }
    }
}
