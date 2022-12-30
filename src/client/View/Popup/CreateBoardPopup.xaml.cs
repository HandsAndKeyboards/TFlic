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

namespace TFlic.View.Popup
{
    /// <summary>
    /// Логика взаимодействия для CreateBoardPopup.xaml
    /// </summary>
    public partial class CreateBoardPopup : Window
    {
        public CreateBoardPopup(object dataContext)
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

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
