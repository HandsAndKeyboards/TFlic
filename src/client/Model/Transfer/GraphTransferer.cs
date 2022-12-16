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
        /// <param name="series"> Данные для построения графа </param>
        public static async void TransferToClient(ObservableCollection<ObservablePoint> redLineValues, ObservableCollection<ObservablePoint> grayLineValues, long idOrganization, long idProject, int sprintNumber)
        {
            int count = 0;
            double max = 0, min = 0;
            Graph graphDto = await WebClient.Get.BurndownGraphAsync(idOrganization, idProject, sprintNumber);
            for (int i = 0; i < graphDto.DateChartValues.Count; i++)
            {
                if (graphDto.DateChartValues.ElementAt(i).Value >= 0)
                {
                    redLineValues.Add(
                        new ObservablePoint(i, graphDto.DateChartValues.ElementAt(i).Value)
                        );
                    count = i;
                }
                else
                {
                    redLineValues.Add(redLineValues.ElementAt(count));
                }

                if (redLineValues.ElementAt(i).Y > max) max = (double)redLineValues.ElementAt(i).Y;
                else if (redLineValues.ElementAt(i).Y < min) min = (double)redLineValues.ElementAt(i).Y;
            }
            for(int i = 0; i < redLineValues.Count; i++)
            {
                grayLineValues.Add(
                    new ObservablePoint(i, max - max / 7 * i)
                    );
            }
        }

        public static async void TransferToClient(ObservableCollection<double> yValues, long idOrganization, long idProject, int sprintNumber)
        {
            Graph graphDto = await WebClient.Get.BurndownGraphAsync(idOrganization, idProject, sprintNumber);
            for (int i = 0; i < graphDto.DateChartValues.Count; i++)
            {
                yValues.Add(graphDto.DateChartValues.ElementAt(i).Value);
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
