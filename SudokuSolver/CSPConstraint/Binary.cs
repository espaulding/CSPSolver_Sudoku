/*
 * Eric Spaulding
 * Professor Alden Wright
 * AI - Fall2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SudokuSolver.Problem;

namespace SudokuSolver.CSPConstraint
{
    public class Binary
    {
        public Cell Xi, Xj;

        public Binary(Cell l, Cell r)
        {
            Xi = l;
            Xj = r;
        }

        public bool Check()
        {
            //false if they are equal
            //true if they are not
            return !(Xi.value == Xj.value);
        }
    }
}
