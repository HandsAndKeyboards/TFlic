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

        private void BMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
