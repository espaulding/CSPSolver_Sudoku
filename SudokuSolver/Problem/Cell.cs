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
    public class Cell
    {
        public int row, column;
        public int value = 0;
        private int[] domain;
        public List<Cell> neighbors = new List<Cell>(); //established by contraints not proximity
        private int _dSize;
        private int domainSize;

        public Cell(int nx, int ny, int domainSize)
        {
            column = nx;
            row = ny;
            domain = new int[domainSize];
            for (int i = 1; i <= domainSize; i++) { domain[i - 1] = i; }
            _dSize = domainSize;
            this.domainSize = domainSize;
        }

        public int dsize
        {
            get { return this._dSize; }
        }

        public void AssignByDomain()
        {
            if (_dSize == 1) {
                foreach (int d in domain) { if (d != 0) { value = d; } }
            }
        }

        public void RemoveFromDomain(int r)
        {
            if (r > 0 && r < 10 && domain[r - 1] != 0) { domain[r - 1] = 0; _dSize--; }
        }

        public void AddToDomain(int r)
        {
            if (r > 0 && r < 10) { domain[r - 1] = r; _dSize++; }
        }

        public void SetValue(int r)
        {
            EmptyDomain();
            AddToDomain(r);
            value = r;
        }

        public void AddArrayToDomain(int[] rs)
        {
            for (int i = 0; i < rs.Length; i++) { AddToDomain(rs[i]); }
        }

        public void EmptyDomain()
        {
            for (int i = 0; i < domainSize; i++) { domain[i] = 0; }
            _dSize = 0;
        }

        public int[] GetDomain()
        {
            return domain;
        }

        public int[] GetDomainCopy()
        {
            int[] output = new int[domain.Length];
            for (int i = 0; i < domain.Length; i++){
                output[i] = domain[i];
            }
            return output;
        }

        public bool SetDomain(int[] d)
        {
            if (d.Length != domain.Length) { return false; }
            _dSize = 0;
            for (int i = 0; i < d.Length; i++){
                domain[i] = d[i];
                if (d[i] != 0) { _dSize++; }
            }
            return true;
        }
    }
}
