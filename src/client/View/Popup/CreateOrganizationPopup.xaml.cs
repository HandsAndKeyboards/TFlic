using System.Windows;
using System.Windows.Input;

namespace TFlic.View
{
    /// <summary>
    /// Логика взаимодействия для CreateOrganizationPopup.xaml
    /// </summary>
    public partial class CreateOrganizationPopup : Window
    {
        public CreateOrganizationPopup(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }

        // Перемещение окна
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
