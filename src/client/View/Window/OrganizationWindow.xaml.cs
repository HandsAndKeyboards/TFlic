using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TFlic.ViewModel;

namespace TFlic.View
{
    /// <summary>
    /// Логика взаимодействия для OrganizationWindow.xaml
    /// </summary>
    public partial class OrganizationWindow : Window
    {
        public OrganizationWindow()
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

        private void AddOrgButton_Click(object sender, RoutedEventArgs e)
        {
            CreateOrganizationPopup createOrganizationPopup = new(DataContext);
            createOrganizationPopup.ShowDialog();
        }

        private void CheckProjects_Click(object sender, RoutedEventArgs e)
        {
            ((OrganizationWindowViewModel)DataContext).IndexOrganization = OrganizationSelecter.SelectedIndex;
            ((OrganizationWindowViewModel)DataContext).CurrentOrganizationsProjects =
                ((OrganizationWindowViewModel)DataContext).Organizations[OrganizationSelecter.SelectedIndex].projects;
        }

        private void ListBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                
            }
        }

        private void ProjectsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((OrganizationWindowViewModel)DataContext).CurrentOrganizationsBoards =
                    ((OrganizationWindowViewModel)DataContext)
                    .Organizations[OrganizationSelecter.SelectedIndex]
                    .projects[ProjectsList.SelectedIndex].boards;
        }

        private void BoardsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BoardWindow boardWindow = new();
        }
    }
}
