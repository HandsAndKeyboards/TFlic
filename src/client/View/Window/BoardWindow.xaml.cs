using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TFlic.ViewModel;
using TFlic.ViewModel.ViewModelClass;

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

        // Обработка событий кнопок интерфейса
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

        private void TaskNextColumn_Click(object sender, RoutedEventArgs e)
        {
            Task t = (Task)((FrameworkElement)sender).DataContext;
            BufferIdColumn.Text = t.IdColumn.ToString();
            BufferIdTask.Text = t.Id.ToString();

            if (((BoardWindowViewModel)DataContext).MoveTaskToNextColumnCommand.CanExecute(sender))
                ((BoardWindowViewModel)DataContext).MoveTaskToNextColumnCommand.Execute(sender);

            BufferIdTask.Clear();
            BufferIdColumn.Clear();
        }

        private void TaskPrevColumn_Click(object sender, RoutedEventArgs e)
        {
            Task t = (Task)((FrameworkElement)sender).DataContext;
            BufferIdColumn.Text = t.IdColumn.ToString();
            BufferIdTask.Text = t.Id.ToString();

            if (((BoardWindowViewModel)DataContext).MoveTaskToPrevColumnCommand.CanExecute(sender))
                ((BoardWindowViewModel)DataContext).MoveTaskToPrevColumnCommand.Execute(sender);

            BufferIdTask.Clear();
            BufferIdColumn.Clear();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            { 
                Task t = (Task)((FrameworkElement)sender).DataContext;
                TaskPopup taskPopup = new(DataContext, t);
                taskPopup.ShowDialog();
            }
        }

        private void ColumnsSettings_Click(object sender, RoutedEventArgs e)
        {
            Column c = (Column)((FrameworkElement)sender).DataContext;
            ColumnPopup columnPopup = new(DataContext, c);
            columnPopup.ShowDialog();
        }

        private void OrganizationButton_Click(object sender, RoutedEventArgs e)
        {
            OrganizationWindow organizationWindow = new();
            organizationWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BurndownWindow burndownWindow = new();
            burndownWindow.Show();
        }
    }
}
