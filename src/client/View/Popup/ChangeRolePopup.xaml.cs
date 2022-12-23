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

namespace TFlic.View.Popup
{
    /// <summary>
    /// Логика взаимодействия для ChangeRolePopup.xaml
    /// </summary>
    public partial class ChangeRolePopup : Window
    {
        public ChangeRolePopup(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;

            userGropusCB.ItemsSource = ((OrganizationWindowViewModel)DataContext)
                .Organizations[((OrganizationWindowViewModel)DataContext).IndexOrganization]
                .userGroups;
        }

        private void ChanheRole_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void BClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddPerson_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (((OrganizationWindowViewModel)DataContext).AddUserInUserGroup.CanExecute(sender))
            {
                ((OrganizationWindowViewModel)DataContext).AddUserInUserGroup.Execute(sender);
                MessageBox.Show("Роль добавлена", "Добавление роли");
            }
            else
            {
                MessageBox.Show("Пользователь уже имеет эту роль", "Добавление роли");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (((OrganizationWindowViewModel)DataContext).RemoveUserInUserGroup.CanExecute(sender))
            {
                ((OrganizationWindowViewModel)DataContext).RemoveUserInUserGroup.Execute(sender);
                MessageBox.Show("Роль удалена", "Удаление роли");
            }
            else
            {
                MessageBox.Show("Пользователь не имеет эту роль", "Удаление роли");
            }
        }
    }
}
