using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TFlic.View.Popup;
using TFlic.ViewModel;
using TFlic.ViewModel.ViewModelClass;

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
            CreateBoard.Visibility = Visibility.Hidden;
            DeleteBoard.Visibility = Visibility.Hidden;
            DeleteProject.Visibility = Visibility.Hidden;
            DeleteUser.Visibility = Visibility.Hidden;
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
            LeftList.UnselectAll();
            DeleteUser.Visibility = Visibility.Hidden;
            LeftList.Visibility = Visibility.Visible;
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
            LeftList.UnselectAll();
            CreateBoard.Visibility = Visibility.Hidden;
            DeleteBoard.Visibility = Visibility.Hidden;
            DeleteProject.Visibility = Visibility.Hidden;
            LeftList.Visibility = Visibility.Visible;
            HeaderLeft.Text = "Список сотрудников";
            HeaderRight.Text = "";

            flagMode = false;

            LeftList.ItemsSource = null;
            RightList.ItemsSource = null;

            LeftList.ItemsSource =
               ((OrganizationWindowViewModel)DataContext)
               .Organizations[OrganizationSelecter.SelectedIndex]
               .peoples;
        }

        private void LeftList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RightList.Visibility = Visibility.Visible;

            ((OrganizationWindowViewModel)DataContext)
                    .IndexSelectedLeftList = LeftList.SelectedIndex;

            if (flagMode && LeftList.SelectedIndex != -1)
            {
                CreateBoard.Visibility = Visibility.Visible;
                DeleteProject.Visibility = Visibility.Visible;

                HeaderRight.Text = "Список досок";
                RightList.ItemsSource = ((OrganizationWindowViewModel)DataContext)
                    .Organizations[OrganizationSelecter.SelectedIndex]
                    .projects[LeftList.SelectedIndex].boards;
            }
            else
            {
                DeleteUser.Visibility = Visibility.Visible;
                ((OrganizationWindowViewModel)DataContext).SelectedUser =
                (Person)LeftList.SelectedItem;
            }
        }

        private void RightList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteBoard.Visibility = Visibility.Visible;
            ((OrganizationWindowViewModel)DataContext)
                    .IndexSelectedBoard = RightList.SelectedIndex;
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

            ((BoardWindowViewModel)boardWindow.DataContext).IdProject =
                ((OrganizationWindowViewModel)DataContext)
                .Organizations[OrganizationSelecter.SelectedIndex]
                .projects[LeftList.SelectedIndex]
                .Id;

            ((BoardWindowViewModel)boardWindow.DataContext).IdBoard =
                ((OrganizationWindowViewModel)DataContext)
                .Organizations[OrganizationSelecter.SelectedIndex]
                .projects[LeftList.SelectedIndex]
                .boards[RightList.SelectedIndex]
                .Id;

            boardWindow.Show();
            Close();
        }

        private void OrgInfo_Click(object sender, RoutedEventArgs e)
        {
            OrganizationPopup organizationPopup = new(DataContext);

            ((OrganizationWindowViewModel)DataContext).OrgName =
                ((OrganizationWindowViewModel)DataContext).Organizations[OrganizationSelecter.SelectedIndex].Name;

            ((OrganizationWindowViewModel)DataContext).OrgDescription =
                ((OrganizationWindowViewModel)DataContext).Organizations[OrganizationSelecter.SelectedIndex].Description;

            organizationPopup.ShowDialog();
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
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddAccountPopup addAccountPopup = new(DataContext);
            addAccountPopup.ShowDialog();
        }

        private void DeleteBoard_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Вы действительно хотите удалить эту доску?",
                "Подтверждение действия",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (((OrganizationWindowViewModel)DataContext).DeleteBoardCommand.CanExecute(result))
            {
                ((OrganizationWindowViewModel)DataContext).DeleteBoardCommand.Execute(sender);
            }
        }

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Вы действительно хотите удалить этот проект?",
                "Подтверждение действия",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (((OrganizationWindowViewModel)DataContext).DeleteProjectCommand.CanExecute(result))
            {
                ((OrganizationWindowViewModel)DataContext).DeleteProjectCommand.Execute(sender);
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Вы действительно хотите исключить этого пользователя?",
                "Подтверждение действия",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (((OrganizationWindowViewModel)DataContext).DeletePersonCommand.CanExecute(result))
            {
                ((OrganizationWindowViewModel)DataContext).DeletePersonCommand.Execute(sender);
            }
        }

        private void OrganizationSelecter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LeftList.Visibility = Visibility.Hidden;
            RightList.Visibility = Visibility.Hidden;
            HeaderLeft.Text = string.Empty;
            HeaderRight.Text = string.Empty;
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Вы действительно хотите выйти из аккаунта?",
                "Подтверждение действия",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);   

            if (((OrganizationWindowViewModel)DataContext).LogoutCommand.CanExecute(result))
            {
                ((OrganizationWindowViewModel)DataContext).LogoutCommand.Execute(sender); 
                AuthorizationWindow authorizationWindow = new();
                authorizationWindow.Show();
                Close();
            }
            
        }

        private void ChangeRoleUser_Click(object sender, RoutedEventArgs e)
        {
            ChangeRolePopup changeRolePopup = new(DataContext);
            changeRolePopup.ShowDialog();
        }

        #endregion


    }
}
