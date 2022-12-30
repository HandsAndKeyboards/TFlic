using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TFlic.ViewModel;

namespace TFlic.View
{
    /// <summary>
    /// Логика взаимодействия для CreateTaskPopup.xaml
    /// </summary>
    public partial class CreateTaskPopup : Window
    {
        public CreateTaskPopup(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
            priorityRB_1.IsChecked = true;
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
            if (((BoardWindowViewModel)DataContext).AddTaskCommand.CanExecute(sender))
            {
                ((BoardWindowViewModel)DataContext).AddTaskCommand.Execute(sender);
                Close();    
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)e.Source;
            ((BoardWindowViewModel)DataContext).ColorPriority = rb.Background.Clone();
            long priority = 0;
            if (priorityRB_1.IsChecked == true) priority = 1;
            if (priorityRB_2.IsChecked == true) priority = 2;
            if (priorityRB_3.IsChecked == true) priority = 3;
            if (priorityRB_4.IsChecked == true) priority = 4;
            if (priorityRB_5.IsChecked == true) priority = 5;
            ((BoardWindowViewModel)DataContext).Priority = priority;
        }
    }
}
