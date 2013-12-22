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
    public class AllDiff : Constraint
    {

        public AllDiff(Cell[] nc)
        {
            constraint = nc;
        }

        public override List<Binary> GetBinaryConstraints()
        {
            if (bConstraints == null)
            {//memoize because Binary Constraints are immutable once established
                bConstraints = new List<Binary>();
                for (int l = 0; l < constraint.Length; l++){
                for (int r = constraint.Length - 1; r >= 0; r--){
                    if (l != r){
                        bConstraints.Add(new Binary(constraint[l], constraint[r]));
                        if (!constraint[l].neighbors.Contains(constraint[r])){
                            constraint[l].neighbors.Add(constraint[r]);
                        }
                        if (!constraint[r].neighbors.Contains(constraint[l])){
                            constraint[r].neighbors.Add(constraint[l]);
                        }
                    }
                }}
            }
            return bConstraints;
        }
    }
}
