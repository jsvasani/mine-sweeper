using UH;
using NUnit.Framework;
using System;
using System.Linq;

namespace UH
{
    [TestFixture]
    public class MineSweeperBoardTest
    {
        MineSweeperBoard _board;

        [SetUp]
        public void TestInitialize()
        {
            _board = new MineSweeperBoard();
            SetDefaultMines();
        }

        [Test]
        public void CanaryTest()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void CheckExposedStatusOfExposedCell()
        {
            _board.ExposeCellAt(0, 0);
            Assert.True(_board.IsCellExposedAt(0, 0));
        }

        [Test]
        public void CheckExposedStatusOfUnexposedCell()
        {
            Assert.False(_board.IsCellExposedAt(0, 0));
        }

        [Test]
        public void CheckIfCellHavingOutOfRangeRowValueIsExposed()
        {
            Assert.False(_board.IsCellExposedAt(10, 0));
        }

        [Test]
        public void CheckIfCellHavingOutOfRangeColumnValueIsExposed()
        {
            Assert.False(_board.IsCellExposedAt(0, 10));
        }

        [Test]
        public void ToggleSealOfCellHavingOutOfRangeRowValueShouldNotChangeAnyCell()
        {
            _board.ToggleSealCellAt(10,0);
            CheckIfAllCellsAreUnsealed();
        }

        [Test]
        public void ToggleSealOfCellHavingOutOfRangeColumnValueShouldNotChangeAnyCell()
        {
            _board.ToggleSealCellAt(0, 10);
            CheckIfAllCellsAreUnsealed();
        }

        [Test]
        public void ToggleSealOfUnsealedCell()
        {
            _board.ToggleSealCellAt(0, 0);
            Assert.True(_board.IsCellSealedAt(0, 0));
        }

        [Test]
        public void ToggleSealofSealedCell()
        {
            _board.ToggleSealCellAt(0, 0);
            _board.ToggleSealCellAt(0, 0);
            Assert.False(_board.IsCellSealedAt(0, 0));
        }

        [Test]
        public void ToggleSealOfExposedCell()
        {
            _board.ExposeCellAt(0, 0);
            _board.ToggleSealCellAt(0, 0);
            Assert.False(_board.sealedCells[0, 0]);
        }

        [Test]
        public void ExposeSealedCell()
        {
            _board.ToggleSealCellAt(0, 0);
            _board.ExposeCellAt(0, 0);
            Assert.False(_board.exposedCells[0, 0]);
        }

        [Test]
        public void ExposeAlreadyExposedCell()
        {
            _board.ExposeCellAt(0, 0);
            _board.ExposeCellAt(0, 0);
            Assert.True(_board.exposedCells[0, 0]);
        }

        [Test]
        public void ExposeMinedCell()
        {
            _board.ExposeCellAt(0, 1);
            Assert.True(_board.exposedCells[0, 1]);
        }

        [Test]
        public void ExposeAdjacentCell()
        {
            _board.ExposeCellAt(0, 0);
            Assert.True(_board.exposedCells[0, 0]);
        }

        public void ExposingTopLeftAdjacentCellShouldExposeOnlyOneCell()
        {
            _board.ExposeCellAt(0, 0);
            Assert.AreEqual(1, GetNumberOfExposedCells());
        }

        [Test]
        public void ExposeEmptyCell()
        {
            _board.ExposeCellAt(0, 1);
            Assert.True(_board.exposedCells[0, 1]);
        }

        [Test]
        public void ExposeEmptyCellAtCenterAndCheckNumberOfExposedCells()
        {
            _board.ExposeCellAt(5, 5);
            Assert.AreEqual(49, GetNumberOfExposedCells());
        }

        [Test]
        public void UpperBoundaryTest()
        {
            Assert.True(_board.IsWithinRange(9));
        }

        [Test]
        public void LowerBoundaryTest()
        {
            Assert.True(_board.IsWithinRange(0));
        }

        [Test]
        public void TestOutofRangePositiveSide()
        {
            Assert.False(_board.IsWithinRange(10));
        }

        [Test]
        public void TestOutofRangeNegativeSide()
        {
            Assert.False(_board.IsWithinRange(-1));
        }

        [Test]
        public void GetValueOfMinedCell()
        {
            Assert.AreEqual("*", _board.GetCellValueAt(0, 1));
        }

        [Test]
        public void GetValueOfEmptyCell()
        {
            Assert.AreEqual(String.Empty, _board.GetCellValueAt(5, 5));
        }

        [Test]
        public void GetValueOfAdjacentCell()
        {
            Assert.AreEqual("3", _board.GetCellValueAt(7, 4));
        }

        [Test]
        public void GetValueOfCellHavingOutOfRangeRowValue()
        {
            Assert.AreEqual(String.Empty, _board.GetCellValueAt(10, 4));
        }

        [Test]
        public void GetValueOfCellHavingOutOfRangeColumnValue()
        {
            Assert.AreEqual(String.Empty, _board.GetCellValueAt(0, 10));
        }

        [Test]
        public void TestTenDistinctLocations()
        {
            int[] locations = _board.GetTenDistinctLocations();
            Assert.AreEqual(10, locations.Distinct().Count(), 
                "Did not return 10 distinct location.");
        }

        [Test]
        public void TestSetMines()
        {
            int[] locations = { -1, 0, 54, 76, 76, 88, 100 };
            _board.SetMinesOnBoard(locations);
            int[,] expectedMinesAtLocation = { { 0, 0 }, { 5, 4 }, { 7, 6 }, { 8, 8 } 
                                             };

            for (int i = 0; i < 4; i++)
            {
                Assert.True(_board.minedCells[expectedMinesAtLocation[i, 0], 
                    expectedMinesAtLocation[i, 1]]);
            }
        }

        [Test]
        public void CountRandomMines()
        {
            _board = new MineSweeperBoard();
            Assert.AreEqual(10, GetNumberOfMines());
        }

        [Test]
        public void TestRandomnessOfMineLocations()
        {
            _board = new MineSweeperBoard();
            MineSweeperBoard board2 = new MineSweeperBoard();
            Assert.False(AreMineLocationdOnBoardsEqual(_board, board2), "Locations are not random.");
        }

        [Test]
        public void GameLostWhenMinedCellExposed()
        {
            _board.ExposeCellAt(0, 1);
            Assert.True(_board.IsGameLost());
        }

        [Test]
        public void GameNotLostTest()
        {
            Assert.False(_board.IsGameLost());
        }

        [Test]
        public void IsAnyMinedCellExposedWhenNoCellsAreExposed()
        {
            Assert.False(_board.IsAnyMinedCellExposed());
        }

        [Test]
        public void IsAnyMinedCellExposedWhenAMinedCellExposed()
        {
            _board.ExposeCellAt(0, 1);
            Assert.True(_board.IsAnyMinedCellExposed());
        }

        [Test]
        public void GameWonWhenAllMinesSealedAndAllOtherCellsExposed()
        {
            SealAllMinedCells();
            ExposeAllNonminedCells();
            Assert.True(_board.IsGameWon());
        }

        [Test]
        public void GameNotWonWhenMinesAreNotSealedAndNonminesAreNotExposed()
        {
            Assert.False(_board.IsGameWon());
        }

        [Test]
        public void GameNotWonWhenMinesAreSealedButNonminesAreNotExposed()
        {
            SealAllMinedCells();
            Assert.False(_board.IsGameWon());
        }

        [Test]
        public void GameNotWonWhenMinesAreNotSealedButNonminesAreExposed()
        {
            ExposeAllNonminedCells();
            Assert.False(_board.IsGameWon());
        }

        [Test]
        public void AreAllMinedCellsSealed()
        {
            SealAllMinedCells();
            Assert.True(_board.AllMinedCellsAreSealed());
        }

        [Test]
        public void AreAllMinedCellsSealedWhenOneMinedCellIsSealed()
        {
            _board.ToggleSealCellAt(0, 1);
            Assert.False(_board.AllMinedCellsAreSealed());
        }

        [Test]
        public void AreAllNonminedCellsExposed()
        {
            ExposeAllNonminedCells();
            Assert.True(_board.AllNonminedCellsAreExposed());
        }

        [Test]
        public void AreAllNonMinedCellsExposedWhenOnlyOneExposed()
        {
            _board.ExposeCellAt(0, 0);
            Assert.False(_board.AllNonminedCellsAreExposed());
        }

        [Test]
        public void MineCellTestForNonMinedCell()
        {
            Assert.False(_board.IsMinedCell(0, 0));
        }

        [Test]
        public void MineCellTestForMinedCell()
        {
            Assert.True(_board.IsMinedCell(0, 1));
        }

        [Test]
        public void MineCellTestForCellHavingOutOfRangeRowValue()
        {
            Assert.False(_board.IsMinedCell(10, 1));
        }

        [Test]
        public void MineCellTestForCellHavingOutOfRangeColumnValue()
        {
            Assert.False(_board.IsMinedCell(1, 10));
        }

        [Test]
        public void GetNumberOfNeighbourMinesForCellWithOneNeighbourMine()
        {
            int actualNeighbourMines = _board.GetNumberOfNeighbourMines(0, 0);
            Assert.AreEqual(1, actualNeighbourMines);
        }

        [Test]
        public void GetNumberOfNeighbourMinesForCellWithNoNeighbourMine()
        {
            int actualNeighbourMines = _board.GetNumberOfNeighbourMines(9, 9);
            Assert.AreEqual(0, actualNeighbourMines);
        }

        [Test]
        public void GetNumberOfNeighbourMinesForCellWithThreeNeighbourMines()
        {
            int actualNeighbourMines = _board.GetNumberOfNeighbourMines(3, 7);
            Assert.AreEqual(3, actualNeighbourMines);
        }

        [Test]
        public void GetNumberOfNeighbourMinesForCellHavingOutOfRangeRowValue()
        {
            int actualNeighbourMines = _board.GetNumberOfNeighbourMines(10,0);
            Assert.AreEqual(0, actualNeighbourMines);
        }

        [Test]
        public void GetNumberOfNeighbourMinesForCellHavingOutOfRangeColumnValue()
        {
            int actualNeighbourMines = _board.GetNumberOfNeighbourMines(0,10);
            Assert.AreEqual(0, actualNeighbourMines);
        }

        [Test]
        public void ExposeNeighboursForCellHavingOutOfRangeRowValue()
        {
            _board.IfEmptyCellExposeAllEmptyAndAdjacentNeighbourCells(10, 0);
            Assert.AreEqual(0, GetNumberOfExposedCells());
        }

        [Test]
        public void ExposeNeighboursForCellHavingOutOfRangeColumnValue()
        {
            _board.IfEmptyCellExposeAllEmptyAndAdjacentNeighbourCells(0, 10);
            Assert.AreEqual(0, GetNumberOfExposedCells());
        }

        [Test]
        public void ExposeNeighboursForAdjacentCell()
        {
            _board.IfEmptyCellExposeAllEmptyAndAdjacentNeighbourCells(0, 0);
            Assert.AreEqual(0, GetNumberOfExposedCells());
        }

        [Test]
        public void ExposeNeighboursForMinedCell()
        {
            _board.IfEmptyCellExposeAllEmptyAndAdjacentNeighbourCells(0, 1);
            Assert.AreEqual(0, GetNumberOfExposedCells());
        }

        [Test]
        public void ExposeNeighboursForEmptyCellAndCheckCount()
        {
            _board.IfEmptyCellExposeAllEmptyAndAdjacentNeighbourCells(5,5);
            Assert.AreEqual(49, GetNumberOfExposedCells());
        }

        [Test]
        public void ExposeNeighboursForEmptyCellAndVerifyCells()
        {
            _board.IfEmptyCellExposeAllEmptyAndAdjacentNeighbourCells(9, 9);
            int[,] expectedExposedCells = { { 5, 6 }, { 6, 6 }, { 7, 6 }, { 8, 6 }, 
                                            { 9, 6 }, { 5, 7 }, { 6, 7 }, { 7, 7 }, 
                                            { 8, 7 }, { 9, 7 }, { 5, 8 }, { 6, 8 }, 
                                            { 7, 8 }, { 8, 8 }, { 9, 8 }, { 5, 9 }, 
                                            { 6, 9 }, { 7, 9 }, { 8, 9 }, { 9, 9 }
                                          };
            CheckIfSpecifiedCellsExposed(expectedExposedCells, 20);
        }

        [Test]
        public void TestRandomSeeds()
        {
            Assert.AreNotEqual(_board.GetRandomSeed(), _board.GetRandomSeed());
        }

        #region Private Methods

        private void SetDefaultMines()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _board.minedCells[i, j] = false;
                }
            }
            _board.minedCells[0, 1] = true;
            _board.minedCells[1, 5] = true;
            _board.minedCells[2, 1] = true;
            _board.minedCells[2, 6] = true;
            _board.minedCells[4, 0] = true;
            _board.minedCells[4, 7] = true;
            _board.minedCells[4, 8] = true;
            _board.minedCells[7, 5] = true;
            _board.minedCells[8, 4] = true;
            _board.minedCells[8, 5] = true;
        }

        private void CheckIfSpecifiedCellsExposed(int[,] cells, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Assert.True(_board.exposedCells[cells[i, 0], cells[i, 1]],
                    "Expected cell is not exposed." + 
                    cells[i, 0].ToString() + 
                    cells[i, 1].ToString());
            }
        }

        private int GetNumberOfExposedCells()
        {
            int actualExposedCellCount = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (_board.exposedCells[i, j])
                    {
                        actualExposedCellCount++;
                    }
                }
            }
            return actualExposedCellCount;
        }

        private void CheckIfAllCellsAreUnsealed()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Assert.False(_board.IsCellSealedAt(i, j));
                }
            }
        }

        private void ExposeAllNonminedCells()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (!_board.minedCells[i, j])
                    {
                        _board.ExposeCellAt(i, j);
                    }
                }
            }
        }

        private void SealAllMinedCells()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (_board.minedCells[i, j])
                    {
                        _board.ToggleSealCellAt(i, j);
                    }
                }
            }
        }

        private bool AreMineLocationdOnBoardsEqual(MineSweeperBoard mineSweeperBoard1, MineSweeperBoard mineSweeperBoard2)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (mineSweeperBoard1.minedCells[i, j] != mineSweeperBoard2.minedCells[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private int GetNumberOfMines()
        {
            int actualCount = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (_board.minedCells[i, j])
                    {
                        actualCount++;
                    }
                }
            }
            return actualCount;
        }

        #endregion
    }
}
