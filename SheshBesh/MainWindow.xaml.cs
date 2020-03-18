﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SheshBesh
{
    public delegate void SpikeClicked(int row, int column, int cube1, int cube2);
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private int cube1;
        private int cube2;
        private Board LogicBoard;
        private UiSpikeElement[,] UiBoard;
        public static event SpikeClicked OnSpikeClicked;
        
        public MainWindow()
        {
            InitializeComponent();
            LogicBoard = new Board();
            UiBoard = new UiSpikeElement[2, 12];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    Spike spike = LogicBoard[i, j];
                    UiBoard[i, j] = new UiSpikeElement(i, j);
                    UiBoard[i, j].MouseDown += OnMouseDown;
                    UiBoard[i, j].MouseEnter += OnMouseEnter;
                    UiBoard[i, j].MouseLeave += OnMouseLeave;
                    SetInGrid(UiBoard[i, j], i, j);
                    UiBoard[i, j].Update(spike.SoldiersCount, spike.Black);
                }
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {            
            if (LogicBoard.FirstClick) { return; }
            Update();
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (LogicBoard.FirstClick) { return; }
            
            UiSpikeElement uiSpikeElement = (UiSpikeElement) sender;
            Spike spike = LogicBoard[uiSpikeElement.Row, uiSpikeElement.Column];
            UiBoard[uiSpikeElement.Row, uiSpikeElement.Column].Update(spike.SoldiersCount + 1, LogicBoard.srcSpike.Spike.Black);
        }

        private void SetInGrid(UiSpikeElement element, int row, int column)
        {
            if (column > 5)
            {
                column++;
            }
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            BoardGrid.Children.Add(element);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            UiSpikeElement uiSpikeElement = (UiSpikeElement) sender;
            OnSpikeClicked?.Invoke(uiSpikeElement.Row, uiSpikeElement.Column, cube1, cube2);
            Update();
        }

        private void Update()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    UiBoard[i, j].Update(LogicBoard[i, j]);
                }
            }
        }

        private void Cubes_Clicked(object sender, RoutedEventArgs e)
        {
            Random cube =new Random();
            cube1 = cube.Next(1, 7);
            cube2 = cube.Next(1, 7);
            TextCube1.Content = cube1;
            TextCube2.Content = cube2;
        }
    }
}