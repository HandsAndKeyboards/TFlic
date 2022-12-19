using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using TFlic.Model;
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
                new LineSeries<ObservablePoint>{},
                new LineSeries<ObservablePoint>{}
            };
        public ISeries[] Series 
        { 
            get => series; 
            set => Set(ref series, value); 
        }

        public ObservableCollection<ObservablePoint> redLineValues = new();
        public ObservableCollection<ObservablePoint> RedLineValues
        {
            get => redLineValues;
            set => Set(ref redLineValues, value);
        }

        ObservableCollection<ObservablePoint> grayLineValues = new();
        public ObservableCollection<ObservablePoint> GrayLineValues
        {
            get => grayLineValues;
            set => Set(ref grayLineValues, value);
        }
        #endregion

        #region Fields

        Sprint currentSprint = new();
        public Sprint CurrentSprint
        {
            get => currentSprint;
            set => Set(ref currentSprint, value);
        }

        int indexSprint = 1;
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

        #endregion

        #region Commands

        public ICommand AddGraphInfo { get; }

        private void OnAddGraphInfoExecuted(object p)
        {
            try
            {
                // - Очищаем
                redLineValues.Clear();
                grayLineValues.Clear();
                // - Берём данные для графа 
                GraphTransferer.TransferToClient(
                    redLineValues,
                    grayLineValues,
                    idOrganization,
                    idProject,
                    indexSprint);
                // - Копируем в массив линий графика линию работы команды
                series[0].Values = redLineValues;
                // - Копируем в массив линий графика идеальную линию
                series[1].Values = grayLineValues;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private bool CanAddGraphInfoExecute(object p) { return true; }

        public ICommand AddSprintInfo { get; }

        private void OnAddSprintInfoExecuted(object p)
        {
            try
            {
                sprints.Clear();
                // - Берём данные о спринтах
                SprintTransferer.TransferToClient(
                    sprints,
                    idOrganization,
                    idProject);
                // - Находим выбранный спринт
                currentSprint = sprints
                    .Where(s => s.Number == indexSprint)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private bool CanAddSprintInfoExecute(object p) { return true; }

        #endregion

        #region Constructors
        public BurndownViewModel()
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
