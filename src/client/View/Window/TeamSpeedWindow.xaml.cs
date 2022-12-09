using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            ((TeamSpeedViewModel)DataContext).IndexEndSprint =
                    EndSprintSelecter.SelectedIndex + 1;
            switch (EndSprintSelecter.SelectedIndex + 1)
            {
                case 1:
                    ((TeamSpeedViewModel)DataContext).Series =
                        new ISeries[]
                        {
                            new ColumnSeries<double>
                            {
                                Name = "Sprint 1",
                                Values = new double[] { 162, 560 }
                            }
                        };
                    ((TeamSpeedViewModel)DataContext).XAxes =
                        new Axis[]
                        {
                            new Axis
                            {
                                Labels = new string[] { "Sprint 1"},
                                LabelsRotation = 15
                            }
                        };
                    break;
                case 2:
                    ((TeamSpeedViewModel)DataContext).Series =
                        new ISeries[]
                        {
                            new ColumnSeries<double>
                            {
                                Name = "Sprint 1",
                                Values = new double[] { 162, 560 }
                            },
                            new ColumnSeries<double>
                            {
                                Name = "Sprint 2",
                                Values = new double[] { 62, 430 }
                            }
                        };
                    ((TeamSpeedViewModel)DataContext).XAxes =
                        new Axis[]
                        {
                            new Axis
                            {
                                Labels = new string[] { "Sprint 1", "Sprint 2"},
                                LabelsRotation = 15
                            }
                        };
                    break;
            }
        }
    }
}
