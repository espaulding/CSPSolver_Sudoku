/*
 * Eric Spaulding
 * Professor Alden Wright
 * AI - Fall2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Problem
{
    public class State
    {
        public Cell[,] board;
        public int domainSize;

        public State(int boardHeight, int boardWidth, int domainSize)
        {
            this.domainSize = domainSize;
            board = new Cell[boardWidth, boardHeight];
            for (int x = 0; x < board.GetLength(0); x++) {
            for (int y = 0; y < board.GetLength(1); y++){
                board[x, y] = new Cell(x+1,y+1,domainSize);
            }}
        }

        public void AssignByDomain()
        {
            for (int x = 0; x < board.GetLength(0); x++){
            for (int y = 0; y < board.GetLength(1); y++){
                board[x, y].AssignByDomain();
            }}
        }

        //public void RemoveFromDomainByCellPosition(int row, int column,int value)
        //{
        //    board[column - 1, row - 1].RemoveFromDomain(value);
        //}

        public Cell[,] GetBoardCopy()
        {
            Cell[,] t = new Cell[9,9];
            for (int x = 0; x < board.GetLength(0); x++){
            for (int y = 0; y < board.GetLength(1); y++){
                t[x, y] = new Cell(x+1,y+1,domainSize);
                t[x, y].value = board[x,y].value;
                t[x, y].SetDomain(board[x,y].GetDomainCopy());
            }}
            return t;
        }

        public void SetBoard(Cell[,] t)
        {
            for (int x = 0; x < board.GetLength(0); x++){
            for (int y = 0; y < board.GetLength(1); y++){
                board[x, y].value = t[x, y].value;
                board[x, y].SetDomain(t[x, y].GetDomainCopy());
            }}
        }

        //public void RotateBoardRight()
        //{

        //}


        public List<Cell> GetUnassignedCells()
        {
            List<Cell> uac = new List<Cell>();
            for (int x = 0; x < board.GetLength(0); x++){
            for (int y = 0; y < board.GetLength(1); y++){
                if (board[x, y].value == 0) { uac.Add(board[x, y]); }
            }}
            return uac;
        }

        //Equals and GetHashCode must be overriden for the Set classes to work correctly
        public override bool Equals(Object rh)
        {
            State rhs = (State)rh;
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (board[x, y].value != rhs.board[x, y].value)
                        return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            string sboard = "";
            for (int y = 0; y < 9; y++)
            {
                if ((y) % 3 == 0 && y != 0) { sboard += "-----------\n"; }
                for (int x = 0; x < 9; x++)
                {
                    if ((x) % 3 == 0 && x != 0) { sboard += "|"; }
                    if (board[x, y].value != 0)
                    {
                        sboard += board[x, y].value;
                    }
                    else
                    {
                        sboard += " ";
                    }
                }
                sboard += "\n";
            }
            return sboard;
        }
    }
}
