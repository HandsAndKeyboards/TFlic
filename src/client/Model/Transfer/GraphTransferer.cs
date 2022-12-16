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
        public static async System.Threading.Tasks.Task TransferToClient(LineSeries<ObservablePoint> LineSeries, long idOrganization, long idProject, int sprintNumber)
        {
            Graph graphDto = await WebClient.Get.BurndownGraphAsync(idOrganization, idProject, sprintNumber);
            ObservablePoint[] values = new ObservablePoint[graphDto.DateChartValues.Count];

            for (int i = 0; i < graphDto.DateChartValues.Count; i++)
            {
                values.Append(
                    new ObservablePoint(i, graphDto.DateChartValues.ElementAt(i).Value)
                    );
            }

            LineSeries.Values = values;

/*            ObservablePoint[] values = new ObservablePoint[graphDto.DateChartValues.Count];
            for(int i = 0; i < graphDto.DateChartValues.Count; i++)
            {
                values.Append(
                    new ObservablePoint(i, graphDto.DateChartValues.ElementAt(i).Value)
                    );
            }

            series.Append(
                new LineSeries<ObservablePoint>
                {
                    Values = values,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 6 },
                    GeometryStroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 6 }
                });*/
        }

        public static async System.Threading.Tasks.Task TransferToClient(ISeries[] series, long idOrganization, long idProject, int sprintStart, int sprintEnd)
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
