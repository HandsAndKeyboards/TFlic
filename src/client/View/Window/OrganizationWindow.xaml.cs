using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TFlic.ViewModel;

namespace TFlic.View
{
    /// <summary>
    /// Логика взаимодействия для OrganizationWindow.xaml
    /// </summary>
    public partial class OrganizationWindow : Window
    {
        // Если true - режим списка проектов/досок
        // Если false - режим списка сотрудников
        bool flagMode = false;
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
            HeaderLeft.Text = "Список проектов";
            flagMode = true;

            LeftList.ItemsSource = null;
            RightList.ItemsSource = null;

            LeftList.ItemsSource =
                ((OrganizationWindowViewModel)DataContext)
                .Organizations[OrganizationSelecter.SelectedIndex]
                .projects;
        }

        private void CheckPeoples_Click(object sender, RoutedEventArgs e)
        {
            HeaderLeft.Text = "Список сотрудников";
            HeaderRight.Text = "";

            flagMode = false;

            LeftList.ItemsSource = null;
            RightList.ItemsSource = null;

            LeftList.ItemsSource =
               ((OrganizationWindowViewModel)DataContext)
               .Organizations[OrganizationSelecter.SelectedIndex]
               .peoples;
            LeftList.UnselectAll();
        }

        private void ListBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {

            }
        }

        private void ProjectsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flagMode && LeftList.SelectedIndex != -1)
            {
                HeaderRight.Text = "Список досок";
                RightList.ItemsSource = ((OrganizationWindowViewModel)DataContext)
                    .Organizations[OrganizationSelecter.SelectedIndex]
                    .projects[LeftList.SelectedIndex].boards;
            }
        }

        private void BoardsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BoardWindow boardWindow = new();
            boardWindow.Show();
            Close();
        }

        private void OrgInfo_Click(object sender, RoutedEventArgs e)
        {
            CreateOrganizationPopup createOrganizationPopup = new(DataContext);
            createOrganizationPopup.ShowDialog();
        }
    }
}
