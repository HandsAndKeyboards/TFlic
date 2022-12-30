using System.Windows;
using System.Windows.Input;

namespace TFlic.View
{
    /// <summary>
    /// Логика взаимодействия для CreateColumnPopup.xaml
    /// </summary>
    public partial class CreateColumnPopup : Window
    {
        public CreateColumnPopup(object p)
        {
            InitializeComponent();
            DataContext = p;
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

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
