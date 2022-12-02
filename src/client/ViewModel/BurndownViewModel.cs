using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TFlic.Command;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.ViewModel
{
    internal class BurndownViewModel : Base.ViewModelBase
    {
        public ISeries[] Series { get; set; }
            = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                    Fill = null
                }
            };

        #region Fields

        int indexSprint = 0;
        public int IndexSprint
        {
            get => indexSprint;
            set => Set(ref indexSprint, value);
        }

        ObservableCollection<Sprint> sprints = new();
        public ObservableCollection<Sprint> Sprints
        {
            get => sprints;
            set => Set(ref sprints, value);
        }
        #endregion


        #region Commands

        #region Команда выбора спринта

        string sprintName = string.Empty;
        public string SprintName
        {
            get => sprintName;
            set => Set(ref sprintName, value);
        }

        public ICommand ChooseSprintCommand { get; }
        private void OnChooseSprintCommandExecuted(object p)
        {
            sprints.Add(
                new Sprint()
                {
                    Name = sprintName,
                }
            );
        }
        private bool CanChooseSprintCommandExecute(object p) { return true; }

        #endregion

        #endregion

        #region Constructors

        public BurndownViewModel()
        {
            ChooseSprintCommand =
                new RelayCommand(OnChooseSprintCommandExecuted);

            TestData();
        }

        #endregion

        #region Tests

        public void TestData()
        {
            sprints.Add(
                new()
                {
                    Name = "1"
                }
            ); 

            sprints.Add(
                new()
                {
                    Name = "2"
                }
            ); 
        }

        #endregion
    }
}
