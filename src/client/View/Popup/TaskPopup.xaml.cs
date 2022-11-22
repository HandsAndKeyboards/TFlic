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
    /// Логика взаимодействия для TaskPopup.xaml
    /// </summary>
    public partial class TaskPopup : Window
    {
        public TaskPopup()
        {
            InitializeComponent();
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            /*RadioButton rb = (RadioButton)e.Source;
            ((BoardWindowViewModel)DataContext).ColorPriority = rb.Background.Clone();*/
        }
    }
}
