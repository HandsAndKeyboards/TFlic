using System.Windows;
using System.Windows.Input;

namespace TFlic.View.Popup
{
    /// <summary>
    /// Логика взаимодействия для AddAccount.xaml
    /// </summary>
    public partial class AddAccountPopup : Window
    {
        public AddAccountPopup(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
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
            Close();
        }
    }
}
