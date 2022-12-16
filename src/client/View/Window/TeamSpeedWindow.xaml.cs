﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using TFlic.ViewModel;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.View
{
    /// <summary>
    /// Логика взаимодействия для GraphWindown.xaml
    /// </summary>
    public partial class TeamSpeedWindow : Window
    {
        public TeamSpeedWindow()
        {
            InitializeComponent();
        }

        // Перемещение окна
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        // Обработка событий кнопок управления окном
        private void BMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void BClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void BFullscreen_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else WindowState = WindowState.Maximized;
        }

        private void OrganizationButton_Click(object sender, RoutedEventArgs e)
        {
            OrganizationWindow organizationWindow = new();
            organizationWindow.Show();
        }

        private void StartSprintList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((TeamSpeedViewModel)DataContext).IndexStartSprint =
                    StartSprintSelecter.SelectedIndex + 1;
        }

        private void EndSprintList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double[] column1 = new double[] { 52, 150 };
            double[] column2 = new double[] { 64, 165 };
            ((TeamSpeedViewModel)DataContext).IndexEndSprint =
                    EndSprintSelecter.SelectedIndex + 1;
            ISeries[] series = new ISeries[EndSprintSelecter.SelectedIndex+1];
            Axis[] xaxes = new Axis[]
            {
                new Axis
                {
                    Labels = new string[EndSprintSelecter.SelectedIndex + 1],
                    LabelsRotation = 15
                }
            };

            for (int i = 0; i <= EndSprintSelecter.SelectedIndex; i++)
            {
                series[i] = new ColumnSeries<double>
                {
                    Name = "Sprint " + (i + 1).ToString(),
                    Values = i == 1 ? column2: column1,
                    // Values = new double[] { ,  }
                };
                xaxes[0].Labels[i] = "Sprint " + (i + 1).ToString();
            }

            ((TeamSpeedViewModel)DataContext).Series = series;
            ((TeamSpeedViewModel)DataContext).XAxes = xaxes;
        }
    }
}