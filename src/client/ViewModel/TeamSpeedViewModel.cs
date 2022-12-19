using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TFlic.Model;
using TFlic.Model.Transfer;
using TFlic.ViewModel.Command;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.ViewModel
{
    internal class TeamSpeedViewModel : Base.ViewModelBase
    {
        #region GraphData
        public ISeries[] series =
            new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Name = "Sprint 1",
                    Values = new double[] { 162, 560 }
                },
                new ColumnSeries<double>
                {
                    Name = "Sprint 2",
                    Values = new double[] { 62, 430 }
                }
            };
        public ISeries[] Series 
        { 
            get => series;
            set => Set(ref series, value); 
        }

        public Axis[] xaxes =
        {
            new Axis
            {
                Labels = new string[] { "Sprint 1", "Sprint 2" },
                LabelsRotation = 15
            }
        };
        public Axis[] XAxes 
        { 
            get => xaxes; 
            set => Set(ref xaxes, value); 
        }
        #endregion

        #region Fields

        Sprint currentSprint = new();
        public Sprint CurrentSprint
        {
            get => currentSprint;
            set => Set(ref currentSprint, value);
        }

        int indexStartSprint = 0;
        public int IndexStartSprint
        {
            get => indexStartSprint;
            set => Set(ref indexStartSprint, value);
        }

        int indexEndSprint = 0;
        public int IndexEndSprint
        {
            get => indexEndSprint;
            set => Set(ref indexEndSprint, value);
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

        #endregion

        #endregion

        #region Constructors

        public TeamSpeedViewModel()
        {
            //GraphTransferer.TransferToClient(series, 2, 1, 1, 2);
        }

        #endregion

    }
}
