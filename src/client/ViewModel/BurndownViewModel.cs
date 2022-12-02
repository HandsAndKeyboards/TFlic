using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TFlic.Command;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.ViewModel
{
    internal class BurndownViewModel : Base.ViewModelBase
    {
        #region GraphData
        public ISeries[] Series { get; set; }
            = new ISeries[]
            {
                new LineSeries<ObservablePoint>
                {
                    Values = new ObservablePoint[]
                    {
                        new ObservablePoint(0, 14),
                        new ObservablePoint(1, 11),
                        new ObservablePoint(2, 7),
                        new ObservablePoint(3, 6),
                        new ObservablePoint(4, 14),
                        new ObservablePoint(5, 4),
                        new ObservablePoint(6, 3),
                        new ObservablePoint(7, 0)
                    }
                },
                new LineSeries<ObservablePoint>
                {
                    Values = new ObservablePoint[]
                    {
                        new ObservablePoint(0, 14),
                        new ObservablePoint(1, 12),
                        new ObservablePoint(2, 10),
                        new ObservablePoint(3, 8),
                        new ObservablePoint(4, 6),
                        new ObservablePoint(5, 4),
                        new ObservablePoint(6, 2),
                        new ObservablePoint(7, 0)
                    }
                }
            };
        #endregion

        #region Fields

        Sprint currentSprint = new();
        public Sprint CurrentSprint
        {
            get => currentSprint;
            set => Set(ref currentSprint, value);
        }

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
                new Sprint()
                {
                    Name = "1"
                }
            ); 

            sprints.Add(
                new Sprint()
                {
                    Name = "2"
                }
            ); 
        }

        #endregion
    }
}
