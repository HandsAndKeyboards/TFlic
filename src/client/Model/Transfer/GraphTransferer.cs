using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlic.Model;
using TFlic.ViewModel;
using TFlic.ViewModel.ViewModelClass;
using TFlic.Model.Transfer;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using LiveChartsCore.Defaults;
using HarfBuzzSharp;
using System.Diagnostics;

namespace TFlic.Model.Transfer
{
    public static class GraphTransferer
    {
        /// <summary>
        /// Передает файлы с сервера клиенту
        /// Также производятся некоторые преобразования, т.к. 
        /// клиент имеет свое средство представлния данных
        /// </summary>
        /// <param name="redLineValues"> Данные о работе команды (время выполнения за день) </param>
        /// <param name="grayLineValues"> Идеальная траектория работы </param>
        public static async void TransferToClient(ObservableCollection<ObservablePoint> redLineValues, ObservableCollection<ObservablePoint> grayLineValues, long idOrganization, long idProject, int sprintNumber)
        {
            // - Cчётчик для дней, когда работали и значения времени изменялись
            int countWorkDays = 0;

            int countOffDays = 0;

            // - Максимальное и минимальное значение времени затраченного на задачи в день командой
            double max = 0, min = 0;

            Graph graphDto = await WebClient.Get.BurndownGraphAsync(idOrganization, idProject, sprintNumber);

            ICollection<Sprint> sprintsDto = await WebClient.Get.SprintsAsync(idOrganization, idProject);
            Sprint currentSprint = sprintsDto
                .Where(s => s.Number == sprintNumber)
                .Select(s => s)
                .FirstOrDefault();

            // - Строим линию работы команды (итерация по дням)
            /*
             TODO: Ошибка когда в первом дне спринта ничего не делали не меняли
            */
            for (var day = currentSprint.BeginDate.Date; day <= currentSprint.EndDate.Date; day = day.AddDays(1))
            {
                Trace.WriteLine("\n->");
                Trace.WriteLine(countWorkDays.ToString());
                Trace.WriteLine(graphDto.DateChartValues.ElementAt(countWorkDays).Point.ToString());
                Trace.WriteLine(day.ToString());
                // - Если в этоим дне работали и значения времени изменялись
                if (graphDto.DateChartValues.ElementAt(countWorkDays).Point == day)
                {
                    redLineValues.Add(
                        new ObservablePoint(countOffDays++, graphDto.DateChartValues.ElementAt(countWorkDays).Value)
                        );

                    if (redLineValues.ElementAt(countWorkDays).Y > max) max = (double)redLineValues.ElementAt(countWorkDays).Y;
                    else if (redLineValues.ElementAt(countWorkDays).Y < min) min = (double)redLineValues.ElementAt(countWorkDays).Y;
                    
                    // - Если ещё есть изменения то идём дальше
                    if(countWorkDays < graphDto.DateChartValues.Count) countWorkDays++;
                    countOffDays = countWorkDays;
                }
                else
                {
                    redLineValues.Add(
                        new ObservablePoint(countOffDays++, graphDto.DateChartValues.ElementAt(countWorkDays-1).Value)
                        );
                }
            }

            // - Строим идеальную линию
            for(int i = 0; i < redLineValues.Count; i++)
            {
                grayLineValues.Add(
                    new ObservablePoint(i, max - max / (redLineValues.Count-1) * i)
                    );
            }
        }

        public static async void TransferToClient(ISeries[] series, long idOrganization, long idProject, int sprintStart, int sprintEnd)
        {
            Graph graphDto = await WebClient.Get.TeamSpeedGraphAsync(idOrganization, idProject, sprintStart, sprintEnd);

            double[] values = new double[graphDto.SprintChartValues.Count];
            for (int i = 0; i < graphDto.DateChartValues.Count; i++)
            {
                values.Append(
                    graphDto.DateChartValues.ElementAt(i).Value
                    );
            }

            series.Append(
                new ColumnSeries<double>
                {
                    Values = values
                });
        }

/*        public static async void TransferToServer(ObservableCollection<Organization> organizations)
        {

        }*/
    }
}
