using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TFlic.ViewModel;

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
            OrganizationWindow organizationWindow = new();
            organizationWindow.Show();
            Close();
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
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            ((TeamSpeedViewModel)DataContext).AddGraphInfo.Execute(sender);
            ((TeamSpeedViewModel)DataContext).AddSprintInfo.Execute(sender);
        }
    }
}
