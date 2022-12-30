using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TFlic.ViewModel;

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
            OrganizationWindow organizationWindow = new();
            this.Close();
            organizationWindow.Show();
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
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            ((BurndownViewModel)DataContext).AddGraphInfo.Execute(sender);
            ((BurndownViewModel)DataContext).AddSprintInfo.Execute(sender);
        }

        private void TeamSpeedButton_Click(object sender, RoutedEventArgs e)
        {
            TeamSpeedWindow teamSpeedWindow = new();

            ((TeamSpeedViewModel)teamSpeedWindow.DataContext).IdOrganization =
                ((BurndownViewModel)DataContext)
                .IdOrganization;
            ((TeamSpeedViewModel)teamSpeedWindow.DataContext).IdProject =
                ((BurndownViewModel)DataContext)
                .IdProject;

            teamSpeedWindow.Show();
            Close();
        }
    }
}
