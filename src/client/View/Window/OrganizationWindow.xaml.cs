using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TFlic.View.Popup;
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

        #region Кнопки управления окном
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
        #endregion

        #region Кнопки панели управления 

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

        private void LeftList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flagMode && LeftList.SelectedIndex != -1)
            {
                CreateBoard.Visibility = Visibility.Visible;
                HeaderRight.Text = "Список досок";
                RightList.ItemsSource = ((OrganizationWindowViewModel)DataContext)
                    .Organizations[OrganizationSelecter.SelectedIndex]
                    .projects[LeftList.SelectedIndex].boards;
            }
        }

        private void RightList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void BoardsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BoardWindow boardWindow = new();
            ((BoardWindowViewModel)boardWindow.DataContext).Columns =
                ((OrganizationWindowViewModel)DataContext)
                .Organizations[OrganizationSelecter.SelectedIndex]
                .projects[LeftList.SelectedIndex]
                .boards[RightList.SelectedIndex]
                .columns;

            ((BoardWindowViewModel)boardWindow.DataContext).IdOrganization =
                ((OrganizationWindowViewModel)DataContext)
                .Organizations[OrganizationSelecter.SelectedIndex]
                .Id;

            ((BoardWindowViewModel)boardWindow.DataContext).IdOrganization =
                ((OrganizationWindowViewModel)DataContext)
                .Organizations[OrganizationSelecter.SelectedIndex]
                .projects[LeftList.SelectedIndex]
                .Id;

            boardWindow.Show();
            Close();
        }

        private void OrgInfo_Click(object sender, RoutedEventArgs e)
        {
            CreateOrganizationPopup createOrganizationPopup = new(DataContext);
            createOrganizationPopup.ShowDialog();
        }

        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            CreateProjectPopup createProjectPopup = new(DataContext);
            createProjectPopup.ShowDialog();
        }
        private void CreateBoard_Click(object sender, RoutedEventArgs e)
        {
            CreateBoardPopup createBoardPopup = new(DataContext);
            createBoardPopup.ShowDialog();
        }

        #endregion

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddAccountPopup addAccountPopup = new();
            addAccountPopup.ShowDialog();
        }
    }
}
