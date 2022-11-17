using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TFlic.View
{
    /// <summary>
    /// Логика взаимодействия для BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        ObservableCollection<object> columns = new();

        public BoardWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

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

        private void AddColumn(object sender, RoutedEventArgs e)
        {
            StackPanel newColumn = new();
            /*
             * 1. Создать метод, который будет создавать шаблонные колонки
             * 2. Создать метод, который будет создаывть шаблонные карточки
             * 3. Создать окошко с карточкой, которое будет выводиться при нажатии на карточку
             * !4. Попробовать организовать карточки с помощью страниц
             */
        }

        private void ColumnBorder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ColumnBorder_DragEnter(object sender, DragEventArgs e)
        {

        }
    }
}
