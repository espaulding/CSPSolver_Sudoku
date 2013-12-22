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
    static public class Algo
    {
        static Random rand = new Random();
        static int boardHeight = 9;
        static int boardWidth = 9;
        static int domainSize = 9;

        public enum Mode
        {
            verbose = 1,
            silent = 2
        };

        private static int guessCount = 0, badGuess = 0;
        public static Mode mode = Mode.silent;

        #region Helper Functions

        static private string Print(int[] d)
        {
            string output = "";
            for (int i = 0; i < d.Length; i++)
            {
                if (d[i] != 0) { output += d[i].ToString() + ","; }
            }
            return output.Substring(0, output.Length - 1);
        }

        static void GetMin(ref double[] list, out double min, out int index)
        {
            min = list[0];
            index = 0;
            for (int i = 1; i < list.Length; i++)
            {
                if (list[i] < min)
                {
                    min = list[i];
                    index = i;
                }
            }
            //adjust so this index doesn't come up as the min next time
            list[index] = 99;
        }

        static private void ResetTies(ref bool[] ties)
        {
            for (int i = 0; i < ties.Length; i++) { ties[i] = false; }
        }

        //similar to the goaltest for search algorithms
        static private bool AssignmentComplete(State assignment)
        {
            bool complete = true;
            for (int x = 0; x < assignment.board.GetLength(0); x++)
            {
                for (int y = 0; y < assignment.board.GetLength(1); y++)
                {
                    if (assignment.board[x, y].value == 0) { complete = false; }
                }
            }
            return complete;
        }

        #endregion

        #region Back Tracking

        static public State BacktrackingSearch(CSP csp)
        {
            guessCount = 0; badGuess = 0;
            State result = BackTrack(csp.current, csp, 1);
            Console.WriteLine("Puzzle solved with {0} guesses, {1} of which were wrong.",guessCount,badGuess);
            return result;
        }

        static private State BackTrack(State assignment, CSP csp, int layer)
        {
            if (AssignmentComplete(assignment)) { return assignment; }
            Cell var = SelectUnassignedVariable(assignment);
            int[] ODV = OrderDomainValues(var, assignment, csp);

            foreach (int value in ODV)
            {
                //((if value is consistent with assignment??)) 
                //why would I pick a value not in var's domain??
                Cell[,] copy = assignment.GetBoardCopy();
                assignment.board[var.column - 1, var.row - 1].SetValue(value); guessCount++;

                if (mode == Mode.verbose) {
                    
                    Console.WriteLine("Guess #{0} Cell {1},{2} is {3} out of {4}, recursion layer {5}", guessCount, var.column, var.row, value, Print(ODV), layer);
                    Console.WriteLine(assignment.ToString());
                    Console.WriteLine("--------------------------------------------");
                }

                CSP inferences = Inference(assignment, var, value);
                if (inferences != null)
                {
                    assignment = inferences.current;
                    if (mode == Mode.verbose) {
                        Console.WriteLine("Inferences from the guess");
                        Console.WriteLine(assignment.ToString());
                        Console.WriteLine("--------------------------------------------");
                    }
                    State result = BackTrack(assignment, csp, layer+1);
                    if (result != null) { return result; }
                } else {
                    if (mode == Mode.verbose) { Console.WriteLine("Deadend guess"); }
                    badGuess++;
                }
                assignment.SetBoard(copy);
            }

            return null;
        }

        static private int[] OrderDomainValues(Cell var, State assignment, CSP csp)
        {
            //the goal here is to pick the domain value of var that rules out the fewest choices
            //among domains of var's neighbors. We'll do this by counting the domain frequencies 
            //of the neighbors and then use that to prioritize the current cell's domain
            int[] domain = var.GetDomain();
            double[] neighborFreq = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (Cell neighbor in var.neighbors)
            {
                foreach (int d in neighbor.GetDomain())
                {
                    if (d != 0) { neighborFreq[d - 1]++; }
                }
            }
            List<int> output = new List<int>();

            //add some probability to the neighborFreq by treating
            //the frequencies as probabilistic weights
            //for (int i = 0; i < domain.Length; i++) {
            //    if (domain[i] != 0) { neighborFreq[i] *= rand.NextDouble(); }
            //}

            double min; int index;
            for (int i = 0; i < domain.Length; i++)
            {
                GetMin(ref neighborFreq, out min, out index);
                if (domain[index] != 0)
                    output.Add(index + 1);
            }
            return output.ToArray();
        }

        static private Cell SelectUnassignedVariable(State assignment)
        {
            List<Cell> unassignedCells = assignment.GetUnassignedCells();
            Cell minDomainSize = unassignedCells[0];
            bool[] ties = new bool[unassignedCells.Count];
            for (int i = 0; i < unassignedCells.Count; i++)
            {
                if (unassignedCells[i].dsize < minDomainSize.dsize)
                {
                    minDomainSize = unassignedCells[i]; ResetTies(ref ties);
                }
                if (unassignedCells[i].dsize == minDomainSize.dsize) { ties[i] = true; }
            }

            //now check if we had any ties
            List<int> tieIndex = new List<int>();
            for (int i = 0; i < ties.Length; i++) { if (ties[i]) { tieIndex.Add(i); } }
            if (tieIndex.Count == 1) { return minDomainSize; }
            else
            {
                int tieBreaker = rand.Next(tieIndex.Count);
                return unassignedCells[tieIndex[tieBreaker]];
            }
        }

        static private CSP Inference(State assignment, Cell var, int value)
        {
            State next = new State(boardHeight,boardWidth,domainSize);
            next.SetBoard(assignment.GetBoardCopy());
            CSP infer = new Sudoku(next,boardHeight,boardWidth,domainSize);
            infer.current.board[var.column - 1, var.row - 1].SetValue(value);
            if (AC3(infer))
            {
                infer.current.AssignByDomain();
                return infer;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region AC-3

        //AC-3 algorithm
        static public bool AC3(CSP csp)
        {
            Queue<Binary> Q = new Queue<Binary>();
            List<Constraint> con = csp.GetConstraints();
            for (int i = 0; i < con.Count; i++) {
                List<Binary> bc = con[i].GetBinaryConstraints();
                for (int c = 0; c < bc.Count; c++) { Q.Enqueue(bc[c]); }
            }

            while (Q.Count > 0){
                Binary bc = Q.Dequeue();
                Cell Xi = bc.Xi, Xj = bc.Xj;
                if (Revise(csp, Xi, Xj)){
                    if (Xi.dsize == 0) { return false; /*inconsistency found*/ }
                    foreach (Cell Xk in Xi.neighbors){
                        if (!Xk.Equals(Xj)){ //skip the neighbor Xj
                            Q.Enqueue(new Binary(Xk, Xi));
                        }
                    }
                }
            }
            return true;
        }

        static private bool Revise(CSP csp, Cell Xi, Cell Xj)
        {
            bool revised = false;
            //anything in a domain that is 0 will be ignored as if not present at all
            foreach (int x in Xi.GetDomain()){
                if (x != 0) {//for each x in Di do
                    bool needsrevise = true;
                    //if no value y in Dj allows (x,y) to satisfy Xi Xj then
                    foreach (int y in Xj.GetDomain()) {
                        if (y != 0) { 
                            if (x != y) { needsrevise = false; } } }
                    if (needsrevise) {
                        Xi.RemoveFromDomain(x); //delete x from Di
                        revised = true;
                    }
                }
            }

            return revised;
        }

        #endregion

        #region PC-2

        //PC-2 algorithm
        //static public bool PC2(CSP csp)
        //{
            //TODO - modify AC-3 into PC-2 so that it properly considers triplets through inference
            //PC-2 (Z,D,C,<)
            //  Q = {(i,k,j)| k in Z ^ i <= j ^ (i!=k!=j)}
            //  While (Q not empty)
            //    pick and delete a path (i,k,j) from Q
            //    if ReviseConstraint((i,k,j),(Z,D,C))
            //      Q = Q + RelatedPaths((i,k,j), |Z|,<)
            //  return(Z,D,C)
        //}

        //static private bool ReviseConstraint((i,k,j),(Z,D,C)){
        //   temp = Cij ^ Cik * Ckk * Ckj
        //   if (temp == Cij) { return false; }
        //   else { Cij = temp; return true; }
        //}

        //static private S RelatedPaths((i,k,j),n,<)
        //{
            //if (i < j)
                //S = {(i,j,m) | (i <= m <= n) ^ (m != j)} +
                //    {(m,i,j) | (1 <= m <= j) ^ (m != i)} +
                //    {(j,i,m) | j < m <= n} +
                //    {(m,j,i) | 1 <= m < i};
            //else
                //S = {(p,i,m) | (1 <= p <= m) ^ (1 <= m <= n)} - {(i,i,i),(k,i,k)};
        //}

        #endregion

    }
}
