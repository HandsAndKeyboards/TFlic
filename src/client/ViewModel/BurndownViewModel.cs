using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using TFlic.Model.Transfer;
using TFlic.ViewModel.Command;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.ViewModel
{
    internal class BurndownViewModel : Base.ViewModelBase
    {
        #region GraphData
        // - Изначально заданы тестовые данные для демонстрации вида графика
        public ISeries[] series
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
        public ISeries[] Series 
        { 
            get => series; 
            set => Set(ref series, value); 
        }

        public LineSeries<ObservablePoint> lineSeries = new();
        public LineSeries<ObservablePoint> LineSeries
        {
            get => (LineSeries<ObservablePoint>)series[0];
            set => Set(ref series[0], value);
        }

        #endregion

        #region Fields

        Model.Sprint currentSprint = new();
        public Model.Sprint CurrentSprint
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

        ObservableCollection<Model.Sprint> sprints = new();
        public ObservableCollection<Model.Sprint> Sprints
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
        #endregion

        #endregion

        #region Constructors

        public BurndownViewModel()
        {
            GraphTransferer.TransferToClient(lineSeries, 2, 1, 1);
            SprintTransferer.TransferToClient(sprints, 2, 1);
            // TestData();
        }

        #endregion
        
        #region Tests

/*        public void TestData()
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
        }*/

        #endregion
    }
}
