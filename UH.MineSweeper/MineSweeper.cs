using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UH
{
    public class MineSweeperBoard
    {
        internal bool[,] minedCells;
        internal bool[,] exposedCells = new bool[10, 10];
        internal bool[,] sealedCells = new bool[10, 10];
 
        public bool IsGameLost()
        {
            return IsAnyMinedCellExposed();
        }

        public bool IsGameWon()
        {
            return AllMinedCellsAreSealed() && AllNonminedCellsAreExposed();
        }

        public MineSweeperBoard()
        {
            SetMinesOnBoard(GetTenDistinctLocations());
        }

        public void ToggleSealCellAt(int row, int column)
        {
            if (IsWithinRange(row) && IsWithinRange(column) && !
                exposedCells[row, column])
            {
                sealedCells[row, column] = !sealedCells[row, column]; 
            }
        }

        public void ExposeCellAt(int row, int column)
        {
            if (IsWithinRange(row) && IsWithinRange(column) && 
                !exposedCells[row, column] && !sealedCells[row, column])
            {
                exposedCells[row, column] = true;
                IfEmptyCellExposeAllEmptyAndAdjacentNeighbourCells(row, column);
            }
        }

        public bool IsCellExposedAt(int row, int column)
        {
            return IsWithinRange(row) && IsWithinRange(column) && 
                exposedCells[row, column];
        }

        public bool IsCellSealedAt(int row, int column)
        {
            return IsWithinRange(row) && IsWithinRange(column) && 
                sealedCells[row, column];
        }

        public string GetCellValueAt(int row, int column)
        {
            if (IsWithinRange(row) && IsWithinRange(column) && minedCells[row, column])
            {
                return "*";
            }
            int mineCount = GetNumberOfNeighbourMines(row, column);
            return mineCount <= 0 ? String.Empty : mineCount.ToString();
        }

        #region Internal Methods

        internal bool IsAnyMinedCellExposed()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (minedCells[i, j] && exposedCells[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal bool AllNonminedCellsAreExposed()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (!minedCells[i, j] && !exposedCells[i, j])
                        return false;
                }
            }
            return true;
        }

        internal bool AllMinedCellsAreSealed()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (minedCells[i, j] && !sealedCells[i, j])
                        return false;
                }
            }
            return true;
        }

        internal void IfEmptyCellExposeAllEmptyAndAdjacentNeighbourCells(
            int row, int column)
        {
            if (IsWithinRange(row) && IsWithinRange(column) && 
                String.IsNullOrEmpty(GetCellValueAt(row, column)))
            {
                for (int i = row - 1; i <= row + 1; i++)
                {
                    for (int j = column - 1; j <= column + 1; j++)
                    {
                        if (!IsMinedCell(i, j))
                            ExposeCellAt(i, j);
                    }
                }
            }
        }

        internal int GetNumberOfNeighbourMines(int row, int column)
        {
            int mineCount = 0;
            if (IsWithinRange(row) && IsWithinRange(column))
            {
                if (IsMinedCell(row - 1, column - 1)) mineCount++;
                if (IsMinedCell(row, column - 1)) mineCount++;
                if (IsMinedCell(row + 1, column - 1)) mineCount++;
                if (IsMinedCell(row - 1, column)) mineCount++;
                if (IsMinedCell(row + 1, column)) mineCount++;
                if (IsMinedCell(row - 1, column + 1)) mineCount++;
                if (IsMinedCell(row, column + 1)) mineCount++;
                if (IsMinedCell(row + 1, column + 1)) mineCount++;
            }
            return mineCount;
        }

        internal bool IsMinedCell(int row, int column)
        {
            return IsWithinRange(row) && IsWithinRange(column) && 
                minedCells[row, column];
        }

        internal bool IsWithinRange(int location)
        {
            return location >= 0 && location < 10;
        }

        internal int[] GetTenDistinctLocations()
        {
            List<int> locations = new List<int>();
            Random random = new Random(GetRandomSeed());
            
            while (locations.Count != 10)
            {
                int randomLocation = random.Next(99);
                if (!locations.Contains(randomLocation))
                    locations.Add(randomLocation);
            }
            return locations.ToArray();
        }

        internal byte GetRandomSeed()
        {
            byte[] seed = new byte[1];
            System.Security.Cryptography.RandomNumberGenerator
                .Create().GetNonZeroBytes(seed);
            return seed[0];
        }

        internal void SetMinesOnBoard(int[] mineLocations)
        {
            minedCells = new bool[10, 10];
            foreach (int location in mineLocations)
            {
                if (location >= 0 && location < 100)
                {
                    int row = location / 10;
                    int col = location % 10;
                    minedCells[row, col] = true;
                }
            }
        }

        #endregion
    }
}
