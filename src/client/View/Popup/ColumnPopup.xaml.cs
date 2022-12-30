using System.Windows;
using System.Windows.Input;
using TFlic.ViewModel;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.View
{
    /// <summary>
    /// Логика взаимодействия для ColumnPopup.xaml
    /// </summary>
    public partial class ColumnPopup : Window
    {
        Column currentColumn;

        public ColumnPopup(object dataContext, Column column)
        {
            InitializeComponent();
            DataContext = dataContext;
            currentColumn = column;
            ((BoardWindowViewModel)DataContext).NameNewColumn = column.Title;
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

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Вы действительно хотите переименовать колонку?",
                "Подтверждение действия",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            ((BoardWindowViewModel)DataContext).IdColumnBuffer = currentColumn.Id.ToString();

            if (((BoardWindowViewModel)DataContext).RenameColumnCommand.CanExecute(result))
            {
                ((BoardWindowViewModel)DataContext).RenameColumnCommand.Execute(sender);
            }

            Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Вы действительно хотите удалить колонку?",
                "Подтверждение действия",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            ((BoardWindowViewModel)DataContext).IdColumnBuffer = currentColumn.Id.ToString();

            if (((BoardWindowViewModel)DataContext).DeleteColumnCommand.CanExecute(result))
            {
                ((BoardWindowViewModel)DataContext).DeleteColumnCommand.Execute(sender);
            }

            Close();
        }
    }
}
