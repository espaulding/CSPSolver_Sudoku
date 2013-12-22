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
using System.Collections;

namespace SudokuSolver.Problem
{
    abstract public class CSP
    {
        public State current;
        protected int boardHeight, boardWidth, domainSize;

        public bool Solve()
        {
            Algo.AC3(this); //make any easy inferences right off the bat
            current.AssignByDomain(); //assign any squares that have had their domain reduced to 1
            current = Algo.BacktrackingSearch(this);
            if (current != null) { return true; }
            return false;
        }

        abstract public List<Constraint> GetConstraints();

        //Equals and GetHashCode must be overriden for the Set classes to work correctly
        public override bool Equals(Object rh)
        {
            CSP rhs = (CSP)rh;
            return this.current.board.Equals(rhs.current.board);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();;
        }

        public override string ToString()
        {
            return this.current.board.ToString();
        }
    }
}
