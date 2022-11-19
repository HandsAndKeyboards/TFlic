using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using TFlic.ViewModel;
using TFlic.ViewModel.ViewModelClasses;

namespace TFlic.View
{
    /// <summary>
    /// Логика взаимодействия для BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        public BoardWindow()
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

        private void ColumnBorder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ColumnBorder_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTaskPopup createTaskPopup = new(DataContext);
            createTaskPopup.ShowDialog();
        }

        private void AddColumnButton_Click(object sender, RoutedEventArgs e)
        {
            CreateColumnPopup createColumnPopup = new(DataContext);
            createColumnPopup.ShowDialog();
        }
    }
}
