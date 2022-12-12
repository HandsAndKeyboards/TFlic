using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using TFlic.ViewModel;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.View
{
    /// <summary>
    /// Логика взаимодействия для GraphWindown.xaml
    /// </summary>
    public partial class BurndownWindow : Window
    {
        public BurndownWindow()
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

        private void SprintList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((BurndownViewModel)DataContext).IndexSprint =
                    SprintSelecter.SelectedIndex + 1;
/*            ObservablePoint[] values = new ObservablePoint[7];*/

/*            for(int i = 0; i <= SprintSelecter.SelectedIndex; i++)
            {
                *//*
                 Взять значение точки(дата + значение) из массива с сервера
                 у данного спринта
                *//*
            }*/
/*                switch(SprintSelecter.SelectedIndex+1)
                {
                case 1:
                    // - Тестовые данные для спринта 1
                    ((BurndownViewModel)DataContext).Series =
                        new ISeries[]
                        {
                            new LineSeries<ObservablePoint>
                            {
                                Values = new ObservablePoint[]
                                {
                                    new ObservablePoint(0, 14),
                                    new ObservablePoint(1, 7),
                                    new ObservablePoint(2, 11),
                                    new ObservablePoint(3, 5),
                                    new ObservablePoint(4, 10),
                                    new ObservablePoint(5, 12),
                                    new ObservablePoint(6, 5),
                                    new ObservablePoint(7, 0)
                                },
                                Fill = null,
                                Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 6 },
                                GeometryStroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 6 }
                            },
                            new LineSeries<ObservablePoint>
                            {
                                Values = new ObservablePoint[]
                                {
                                    new ObservablePoint(0, 14),
                                    new ObservablePoint(1, 12),
                                    new ObservablePoint(2, 10),
                                    new ObservablePoint(3, 8),
                                    new ObservablePoint(4, 6),
                                    new ObservablePoint(5, 4),
                                    new ObservablePoint(6, 2),
                                    new ObservablePoint(7, 0)
                                },
                                Fill = null,
                                Stroke = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 6 },
                                GeometrySize = 5,
                                GeometryStroke = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 5 }
                            }
                        };
                    break;
                case 2:
                    // - Тестовые данные для спринта 2
                    ((BurndownViewModel)DataContext).Series =
                        new ISeries[]
                        {
                            new LineSeries<ObservablePoint>
                            {
                                Values = new ObservablePoint[]
                                {
                                    new ObservablePoint(0, 14),
                                    new ObservablePoint(1, 12),
                                    new ObservablePoint(2, 10),
                                    new ObservablePoint(3, 5),
                                    new ObservablePoint(4, 6),
                                    new ObservablePoint(5, 5),
                                    new ObservablePoint(6, 2),
                                    new ObservablePoint(7, 0)
                                },
                                Fill = null,
                                Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 6 },
                                GeometryStroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 6 }
                            },
                            new LineSeries<ObservablePoint>
                            {
                                Values = new ObservablePoint[]
                                {
                                    new ObservablePoint(0, 14),
                                    new ObservablePoint(1, 12),
                                    new ObservablePoint(2, 10),
                                    new ObservablePoint(3, 8),
                                    new ObservablePoint(4, 6),
                                    new ObservablePoint(5, 4),
                                    new ObservablePoint(6, 2),
                                    new ObservablePoint(7, 0)
                                },
                                Fill = null,
                                Stroke = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 6 },
                                GeometrySize = 5,
                                GeometryStroke = new SolidColorPaint(SKColors.Gray) { StrokeThickness = 5 }
                            }
                        };
                    break;
                }*/
        }
    }
}
