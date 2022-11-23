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
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.View
{
    /// <summary>
    /// Логика взаимодействия для TaskPopup.xaml
    /// </summary>
    public partial class TaskPopup : Window
    {
        ViewModel.ViewModelClass.Task currentTask;
        public TaskPopup(object dataContext, ViewModel.ViewModelClass.Task task)
        {
            InitializeComponent();
            DataContext = dataContext;
            currentTask = task;
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
            RadioButton rb = (RadioButton)e.Source;
            ((BoardWindowViewModel)DataContext).ColorPriority = rb.Background.Clone();
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Вы действительно хотите изменить содержимое задачи?",
                "Подтверждение действия", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question,
                MessageBoxResult.No);

            ((BoardWindowViewModel)DataContext).IdTaskBuffer = currentTask.Id.ToString();
            ((BoardWindowViewModel)DataContext).IdColumnBuffer = currentTask.IdColumn.ToString();

            if (((BoardWindowViewModel)DataContext).ChangeTaskCommand.CanExecute(result))
            {
                ((BoardWindowViewModel)DataContext).ChangeTaskCommand.Execute(sender);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Вы действительно хотите удалить карточку с задачей?",
                "Подтверждение действия",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            ((BoardWindowViewModel)DataContext).IdTaskBuffer = currentTask.Id.ToString();
            ((BoardWindowViewModel)DataContext).IdColumnBuffer = currentTask.IdColumn.ToString();

            if (((BoardWindowViewModel)DataContext).DeleteTaskCommand.CanExecute(result))
            {
                ((BoardWindowViewModel)DataContext).DeleteTaskCommand.Execute(sender);
            }
            Close();
        }
    }
}
