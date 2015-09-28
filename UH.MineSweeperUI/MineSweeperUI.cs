using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UH
{
    public partial class MineSweeperUI : Form
    {
        MineSweeperBoard _board;
        const int _gridSize = 10;
        Button[,] _cells = new Button[_gridSize, _gridSize];
        const int _cellSize = 35;
        const int _fontSize = 14;
 
        public MineSweeperUI()
        {
            InitializeComponent();
            CreateCells();
            StartNewGame();
        }

        private void CreateCells()
        {
            for (int row = 0; row < _gridSize; row++)
            {
                for (int column = 0; column < _gridSize; column++)
                {
                    _cells[row, column] = GetCellForLocation(row, column);
                    pnlGame.Controls.Add(_cells[row, column]);
                }
            }
        }

        private Button GetCellForLocation(int row, int column)
        {
            Button cell = new Button();
            cell.Font = new Font(this.Font.FontFamily, _fontSize, this.Font.Style);
            cell.Width = cell.Height = _cellSize;
            cell.Location = new Point(row * _cellSize, column * _cellSize);
            cell.MouseDown += new MouseEventHandler(CellClick);
            return cell;
        }

        private void StartNewGame()
        {
            _board = new MineSweeperBoard();
            pnlGame.Enabled = true;
            ResetCells();
        }

        private void ResetCells()
        {
            for (int row = 0; row < _gridSize; row++)
            {
                for (int column = 0; column < _gridSize; column++)
                {
                    _cells[row, column].Text = String.Empty;
                    _cells[row, column].BackColor = buttonNewGame.BackColor;
                    _cells[row, column].Enabled = true;
                }
            }
        }

        private void ButtonNewGameClick(object sender, EventArgs e)
        {
            StartNewGame();
        }

        void CellClick(object sender, EventArgs e)
        {
            for (int row = 0; row < _gridSize; row++)
            {
                for (int column = 0; column < _gridSize; column++)
                {
                    if (_cells[row, column] == (Button)sender)
                    {
                        HandleButtonClick(((MouseEventArgs)e).Button, row, column);
                        CheckForGameEnd(row, column);
                    }
                }
            }
        }

        private void CheckForGameEnd(int lastClickedCellRow, int lastClickedCellColumn)
        {
            if (_board.IsGameLost())
            {
                _cells[lastClickedCellRow, lastClickedCellColumn].BackColor = Color.Red;
                pnlGame.Enabled = false;
                MessageBox.Show("You Lost.", "MineSweeper");
            }
            else if (_board.IsGameWon())
            {
                    pnlGame.Enabled = false;
                    MessageBox.Show("You Won.", "MineSweeper");
            }
        }

        private void HandleButtonClick(MouseButtons buttonClicked, int cellRow, int cellColumn)
        {
            switch (buttonClicked)
            {
                case MouseButtons.Left: HandleExposeCell(cellRow, cellColumn); break;
                case MouseButtons.Right: HandleSealUnSealCell(cellRow, cellColumn); break;
            }
        }

        private void HandleExposeCell(int row, int column)
        {
            _board.ExposeCellAt(row, column);
            ShowValueOfExposedCellsAndDisableThem();
        }

        private void HandleSealUnSealCell(int row, int column)
        {
            _board.ToggleSealCellAt(row, column);
            _cells[row, column].Text = String.Empty;
            if (_board.IsCellSealedAt(row, column))
            {
                _cells[row, column].Text = "S";
            }
        }

        private void ShowValueOfExposedCellsAndDisableThem()
        {
            for (int row = 0; row < _gridSize; row++)
            {
                for (int column = 0; column < _gridSize; column++)
                {
                    if (_board.IsCellExposedAt(row, column))
                    {
                        _cells[row, column].Text = _board.GetCellValueAt(row, column);
                        _cells[row, column].BackColor = Color.White;
                        _cells[row, column].Enabled = false;
                    }
                }
            }
        }
    }
}
