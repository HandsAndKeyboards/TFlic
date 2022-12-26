#pragma warning disable CS4014

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using TFlic.Model;

using ThreadingTask = System.Threading.Tasks.Task;
using TFlic.ViewModel.Command;
using LiveChartsCore.Defaults;
using TFlic.Model.Transfer;
using TFlic.ViewModel.Service;

namespace TFlic.ViewModel
{
    internal class TeamSpeedViewModel : Base.ViewModelBase
    {
        #region GraphData
        public ISeries[] series =
            new ISeries[]
            {
                new ColumnSeries<double>{},
                new ColumnSeries<double>{},
            };

        public ISeries[] Series
        {
            get => series;
            set => Set(ref series, value);
        }

        public Axis[] xaxes =
        {
            new Axis{}
        };

        public Axis[] XAxes
        {
            get => xaxes;
            set => Set(ref xaxes, value);
        }

        public ObservableCollection<string> labels = new ObservableCollection<string>();
        public ObservableCollection<string> Labels
        {
            get => labels;
            set => Set(ref labels, value);
        }

        public ObservableCollection<double> redLineValues = new();
        public ObservableCollection<double> RedLineValues
        {
            get => redLineValues;
            set => Set(ref redLineValues, value);
        }

        ObservableCollection<double> grayLineValues = new();
        public ObservableCollection<double> GrayLineValues
        {
            get => grayLineValues;
            set => Set(ref grayLineValues, value);
        }
        #endregion

        #region Fields

        Sprint currentStartSprint = new();
        public Sprint CurrentStartSprint
        {
            get => currentStartSprint;
            set => Set(ref currentStartSprint, value);
        }

        Sprint currentEndSprint = new();
        public Sprint CurrentEndSprint
        {
            get => currentEndSprint;
            set => Set(ref currentEndSprint, value);
        }

        int indexStartSprint = 1;
        public int IndexStartSprint
        {
            get => indexStartSprint;
            set => Set(ref indexStartSprint, value);
        }

        int indexEndSprint = 2;
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

        long idOrganization = 0;
        public long IdOrganization
        {
            get => idOrganization;
            set => Set(ref idOrganization, value);
        }

        long idProject = 0;
        public long IdProject
        {
            get => idProject;
            set
            {
                Set(ref idProject, value);
                NotifyPropertyChanged();
            }
        }

        #region Commands

        public ICommand AddGraphInfo { get; }

        private async void OnAddGraphInfoExecuted(object p)
        {
            try
            {
                // - Очищаем
                redLineValues.Clear();
                grayLineValues.Clear();
                // - Берём данные для графа 
                await GraphTransferer.TransferToClient(
                    labels,
                    redLineValues,
                    grayLineValues,
                    idOrganization,
                    idProject,
                    IndexStartSprint,
                    IndexEndSprint);
                // - Копируем в массив линий графика линию работы команды
                series[0].Values = redLineValues;
                // - Копируем в массив линий графика идеальную линию
                series[1].Values = grayLineValues;
                // - Лейблы на оси Х
                XAxes[0].Labels = labels;
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
        }

        private bool CanAddGraphInfoExecute(object p) { return true; }

        public ICommand AddSprintInfo { get; }

        private async void OnAddSprintInfoExecuted(object p)
        {
            try
            {
                sprints.Clear();
                // - Берём данные о спринтах
                await SprintTransferer.TransferToClient(
                    sprints,
                    idOrganization,
                    idProject);
                // - Находим выбранный спринт
                currentStartSprint = sprints
                    .Where(s => s.Number == indexStartSprint)
                    .FirstOrDefault();
                currentEndSprint = sprints
                    .Where(s => s.Number == indexEndSprint)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(ex);
            }
        }

        private bool CanAddSprintInfoExecute(object p) { return true; }
        #endregion

        #region Constructors

        public TeamSpeedViewModel()
        {
            AddGraphInfo = new RelayCommand(
                OnAddGraphInfoExecuted,
                CanAddGraphInfoExecute);
            AddSprintInfo = new RelayCommand(
                OnAddSprintInfoExecuted,
                CanAddSprintInfoExecute
                );
        }

        #endregion

        #region Methods
        private void NotifyPropertyChanged()
        {
            AddGraphInfo.Execute(this);
            AddSprintInfo.Execute(this);
        }
        #endregion

    }
}
