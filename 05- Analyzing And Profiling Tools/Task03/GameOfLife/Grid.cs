﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    class Grid
    {
        private Canvas drawCanvas;
        private Random rnd;
        
        private int SizeX;
        private int SizeY;
        
        private Cell[,] cells;
        private Cell[,] nextGenerationCells;
        private Ellipse[,] cellsVisuals;

        
        public Grid(Canvas c)
        {
            drawCanvas = c;
            rnd = new Random();
            
            SizeX = (int) (c.Width / 5);
            SizeY = (int)(c.Height / 5);
            
            cells = new Cell[SizeX, SizeY];
            nextGenerationCells = new Cell[SizeX, SizeY];
            cellsVisuals = new Ellipse[SizeX, SizeY];
            
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j] = new Cell(i, j, 0, GetRandomBoolean());

                    nextGenerationCells[i, j] = new Cell(i, j, 0, false);
                    
                    cellsVisuals[i, j] = new Ellipse();
                    cellsVisuals[i, j].Width = cellsVisuals[i, j].Height = 5;
                    cellsVisuals[i, j].Margin = new Thickness(cells[i, j].PositionX, cells[i, j].PositionY, 0, 0);
                    cellsVisuals[i, j].Fill = GetEllipseFillColor(cells[i, j]);
                    drawCanvas.Children.Add(cellsVisuals[i, j]);

                    cellsVisuals[i, j].MouseMove += MouseMove;
                    cellsVisuals[i, j].MouseLeftButtonDown += MouseMove;
                }
        }
        
        public void Update()
        {
            bool alive = false;
            int age = 0;

            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    //nextGenerationCells[i, j] = CalculateNextGeneration(i,j);          // UNOPTIMIZED
                    CalculateNextGeneration(i, j, ref alive, ref age);   // OPTIMIZED
                    nextGenerationCells[i, j].IsAlive = alive;  // OPTIMIZED
                    nextGenerationCells[i, j].Age = age;  // OPTIMIZED

                    cells[i, j].IsAlive = alive;
                    cells[i, j].Age = age;
                    
                    cellsVisuals[i, j].Fill = GetEllipseFillColor(cells[i, j]);
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                {
                    cells[i, j].Age = 0;
                    cells[i, j].IsAlive = false;

                    nextGenerationCells[i, j].Age = 0;
                    nextGenerationCells[i, j].IsAlive = false;

                    cellsVisuals[i, j].Fill = Brushes.Gray;
                }
        }
        
        
        #region Helper
        private bool GetRandomBoolean()
        {
            return rnd.NextDouble() > 0.8;
        }
        private SolidColorBrush GetEllipseFillColor(Cell cell)
        {
            return cell.IsAlive 
                ? (cell.Age < 2 ? Brushes.White : Brushes.DarkGray)
                : Brushes.Gray;
        }
        void MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Ellipse cellVisual)
            {
                int i = (int) cellVisual.Margin.Left / 5;
                int j = (int) cellVisual.Margin.Top / 5;
            
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (!cells[i, j].IsAlive)
                    {
                        cells[i, j].IsAlive = true;
                        cells[i, j].Age = 0;
                        cellVisual.Fill = Brushes.White;
                    }
                }
            }
        }
        private void CalculateNextGeneration(int row, int column, ref bool isAlive, ref int age)     // OPTIMIZED
        {
            isAlive = cells[row, column].IsAlive;
            age = cells[row, column].Age;

            int count = CountNeighbors(row, column);

            if (isAlive && count < 2)
            {
                isAlive = false;
                age = 0;
            }

            if (isAlive && (count == 2 || count == 3))
            {
                cells[row, column].Age++;
                isAlive = true;
                age = cells[row, column].Age;
            }

            if (isAlive && count > 3)
            {
                isAlive = false;
                age = 0;
            }

            if (!isAlive && count == 3)
            {
                isAlive = true;
                age = 0;
            }
        }
        private int CountNeighbors(int i, int j)
        {
            int count = 0;

            if (i != SizeX - 1 && cells[i + 1, j].IsAlive) count++;
            if (i != SizeX - 1 && j != SizeY - 1 && cells[i + 1, j + 1].IsAlive) count++;
            if (j != SizeY - 1 && cells[i, j + 1].IsAlive) count++;
            if (i != 0 && j != SizeY - 1 && cells[i - 1, j + 1].IsAlive) count++;
            if (i != 0 && cells[i - 1, j].IsAlive) count++;
            if (i != 0 && j != 0 && cells[i - 1, j - 1].IsAlive) count++;
            if (j != 0 && cells[i, j - 1].IsAlive) count++;
            if (i != SizeX - 1 && j != 0 && cells[i + 1, j - 1].IsAlive) count++;

            return count;
        }
        #endregion
    }
}