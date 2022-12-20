#pragma warning disable CS4014

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using TFlic.Model;

using ThreadingTask = System.Threading.Tasks.Task;

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
            LoadData();
        }

        #endregion

        #region Methods
        private async ThreadingTask LoadData()
        {
            /*
             * Подгрузка данных должна осуществляться в этом методе (по крайней мере не в констркуторе).
             *
             * Для возможности отлова исключений необходимо дожидаться окончания выполнения метода загрузки данных,
             * иначе вызывающий метод будет завершаться раньше, чем вызываемый выкинет исключение => оно не попадет
             * в блок catch. Для предотвращения выхода из метода раньше, чем будет поймано исключение, необходимо
             * либо использовать оператор await, либо Task::Wait(). Первый в конструкторе использовать нельзя,
             * второй же при использовании в конструкторе намертво останавливает выполнение проги (по крайней мере
             * такая ситуация происходила в OrganizationWindowViewModel). Чтобы решить возникшую проблему, загрузка
             * данных с обработкой исключений вызывается из конструктора, и обычно завершает работу после выполнения
             * констуктора, и не замораживает программу. 
             */
            
            //await GraphTransferer.TransferToClient(series, 2, 1, 1, 2);
        }
        #endregion

    }
}
