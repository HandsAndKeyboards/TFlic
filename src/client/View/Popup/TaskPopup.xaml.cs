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

            ((BoardWindowViewModel)DataContext).NameTask = task.Name;
            ((BoardWindowViewModel)DataContext).DescriptionTask = task.Description;
            ((BoardWindowViewModel)DataContext).ExecutionTime = task.ExecutionTime;
            ((BoardWindowViewModel)DataContext).Deadline = task.DeadLine;
            ((BoardWindowViewModel)DataContext).Login = task.LoginExecutor;

            switch (task.Priority)
            {
                case 1: priorityRB_1.IsChecked = true; break;
                case 2: priorityRB_2.IsChecked = true; break;
                case 3: priorityRB_3.IsChecked = true; break;
                case 4: priorityRB_4.IsChecked = true; break;
                case 5: priorityRB_5.IsChecked = true; break;
                default: priorityRB_1.IsChecked = true; break;
            }
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
            long priority = 0;
            if (priorityRB_1.IsChecked == true) priority = 1;
            if (priorityRB_2.IsChecked == true) priority = 2;
            if (priorityRB_3.IsChecked == true) priority = 3;
            if (priorityRB_4.IsChecked == true) priority = 4;
            if (priorityRB_5.IsChecked == true) priority = 5;
            ((BoardWindowViewModel)DataContext).Priority = priority;
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

            ((BoardWindowViewModel)DataContext).NameTask = NameTaskTB.Text;
            ((BoardWindowViewModel)DataContext).DescriptionTask = DescTaskTB.Text;
            ((BoardWindowViewModel)DataContext).ExecutionTime = Convert.ToInt32(ExexcutionTimeTB.Text);
            ((BoardWindowViewModel)DataContext).Deadline = Convert.ToDateTime(DeadlineDP.Text);
            ((BoardWindowViewModel)DataContext).NameExecutor = NameExecutorTB.Text;

            long priority = 0;
            if (priorityRB_1.IsChecked == true) priority = 1;
            if (priorityRB_2.IsChecked == true) priority = 2;
            if (priorityRB_3.IsChecked == true) priority = 3;
            if (priorityRB_4.IsChecked == true) priority = 4;
            if (priorityRB_5.IsChecked == true) priority = 5;
            ((BoardWindowViewModel)DataContext).Priority = priority;

            if (((BoardWindowViewModel)DataContext).ChangeTaskCommand.CanExecute(result))
            {
                ((BoardWindowViewModel)DataContext).ChangeTaskCommand.Execute(sender);
                Close();
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
