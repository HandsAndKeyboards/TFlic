using System.Windows;
using System.Windows.Input;
using TFlic.ViewModel;

namespace TFlic.View.Popup
{
    /// <summary>
    /// Логика взаимодействия для OrganizationPopup.xaml
    /// </summary>
    public partial class OrganizationPopup : Window
    {
        public OrganizationPopup(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton== MouseButtonState.Pressed) 
                DragMove();
        }

        private void BClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Вы действительно хотите изменить данные организации?",
                "Подтверждение действия",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (((OrganizationWindowViewModel)DataContext).ChangeOrgInfoCommand.CanExecute(result))
            {
                ((OrganizationWindowViewModel)DataContext).ChangeOrgInfoCommand.Execute(sender);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
