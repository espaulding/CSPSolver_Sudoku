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
    public abstract class Constraint
    {
        protected Cell[] constraint;
        protected List<Binary> bConstraints = null;

        abstract public List<Binary> GetBinaryConstraints();

        public bool check() //only pass if all binary constrains pass
        {
            List<Binary> bc = GetBinaryConstraints();
            for (int i = 0; i < bc.Count; i++)
            {
                if (bc[i].Check() == false) { return false; }
            }
            return true;
        }
    }
}
