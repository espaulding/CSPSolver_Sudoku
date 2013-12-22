/*
 * Eric Spaulding
 * Professor Alden Wright
 * AI - Fall2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SudokuSolver.CSPConstraint;

namespace SudokuSolver.Problem
{
    public class Sudoku : CSP
    {
        public List<Constraint> constraints = null;

        public Sudoku(int boardHeight, int boardWidth, int domainSize)
        {
            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;
            this.domainSize = domainSize;
            current = new State(boardWidth,boardHeight,domainSize);
            constraints = GetConstraints();
        }

        public Sudoku(State ns, int boardHeight, int boardWidth, int domainSize)
        {
            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;
            this.domainSize = domainSize;
            current = ns;
            constraints = GetConstraints();
        }

        public override List<Constraint> GetConstraints()
        {
            if (constraints == null)
            {
                //get the 27 all diff constraints of sudoku
                constraints = new List<Constraint>();

                //columns
                for (int x = 0; x < boardHeight; x++) {
                    Cell[] row = new Cell[boardWidth];
                    for (int y = 0; y < boardWidth; y++) {
                        row[y] = current.board[y, x];
                    }
                    constraints.Add(new AllDiff(row));
                }

                //rows
                for (int y = 0; y < boardWidth; y++) {
                    Cell[] row = new Cell[boardHeight];
                    for (int x = 0; x < boardHeight; x++) {
                        row[x] = current.board[y, x];
                    }
                    constraints.Add(new AllDiff(row));
                }

                //subsquares
                for (int sx = 0; sx < boardWidth; sx += 3) {
                    for (int sy = 0; sy < boardHeight; sy += 3) {
                        constraints.Add(new AllDiff(new Cell[] { 
                        current.board[sy    , sx], current.board[sy    , sx + 1], current.board[sy    , sx + 2], 
                        current.board[sy + 1, sx], current.board[sy + 1, sx + 1], current.board[sy + 1, sx + 2], 
                        current.board[sy + 2, sx], current.board[sy + 2, sx + 1], current.board[sy + 2, sx + 2], 
                        })); 
                } }
            }
            return constraints;
        }
    }
}
