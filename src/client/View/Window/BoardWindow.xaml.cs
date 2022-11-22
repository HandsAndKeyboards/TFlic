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
        Color priority1 { get; set; }
        Color priority2 { get; set; }
        Color priority3 { get; set; }
        Color priority4 { get; set; }
        Color priority5 { get; set; }

        public BoardWindow()
        {
            InitializeComponent();
            priority1 = Color.FromRgb(1, 220, 12);
            priority2 = Color.FromRgb(220, 220, 0);
            priority3 = Color.FromRgb(235, 150, 0);
            priority4 = Color.FromRgb(235, 85, 0);
            priority5 = Color.FromRgb(245, 10, 0);
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

        private void Tasks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            /*TaskPopup taskPopup = new(DataContext);
            taskPopup.ShowDialog();*/
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
    }
}
