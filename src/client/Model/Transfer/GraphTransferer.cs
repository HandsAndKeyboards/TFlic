using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using TFlic.Model.Service;
using AuthenticationManager = TFlic.Model.Authentication.AuthenticationManager;
using ThreadingTask = System.Threading.Tasks.Task;

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
        public static async ThreadingTask TransferToClient(ObservableCollection<ObservablePoint> redLineValues, ObservableCollection<ObservablePoint> grayLineValues, long idOrganization, long idProject, int sprintNumber)
        {
            // - Cчётчик для дней, когда работали и значения времени изменялись
            int countWorkDays = 0;

            int countOffDays = 0;

            // - Максимальное и минимальное значение времени затраченного на задачи в день командой
            double max = 0, min = 0;
            
            Graph? graphDto = null;
            try
            {
                graphDto = await WebClient.Get.BurndownGraphAsync(idOrganization, idProject, sprintNumber); 
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    graphDto = await WebClient.Get.BurndownGraphAsync(idOrganization, idProject, sprintNumber); 
                }
            }
            if (graphDto is null) { throw new NullReferenceException(); }
            
            ICollection<Sprint>? sprintsDto = null;
            try
            {
                sprintsDto = await WebClient.Get.SprintsAsync(idOrganization, idProject); 
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    sprintsDto = await WebClient.Get.SprintsAsync(idOrganization, idProject); 
                }
            }
            if (sprintsDto is null) { throw new NullReferenceException(); }
            
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

        public static async ThreadingTask TransferToClient(ObservableCollection<string> XAxes, ObservableCollection<double> redLineValues, ObservableCollection<double> grayLineValues, long idOrganization, long idProject, int startIndexSprint, int endIndexSprint)
        {

            Graph? graphDto = null;
            try
            {
                graphDto = await WebClient.Get.TeamSpeedGraphAsync(idOrganization, idProject, startIndexSprint, endIndexSprint);
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    graphDto = await WebClient.Get.TeamSpeedGraphAsync(idOrganization, idProject, startIndexSprint, endIndexSprint);
                }
            }
            if (graphDto is null) { throw new NullReferenceException(); }

            ICollection<Sprint>? sprintsDto = null;
            try
            {
                sprintsDto = await WebClient.Get.SprintsAsync(idOrganization, idProject);
            }
            catch (ApiException err)
            {
                if (err.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    var account = AccountService.ReadAccountFromJsonFile();
                    AuthenticationManager.Refresh(account.Tokens.RefreshToken, account.Login);
                    sprintsDto = await WebClient.Get.SprintsAsync(idOrganization, idProject);
                }
            }
            if (sprintsDto is null) { throw new NullReferenceException(); }

            Sprint currentStartSprint = sprintsDto
                .Where(s => s.Number == startIndexSprint)
                .Select(s => s)
                .FirstOrDefault();
            Sprint currentEndSprint = sprintsDto
                .Where(s => s.Number == endIndexSprint)
                .Select(s => s)
                .FirstOrDefault();

            for (int i = currentStartSprint.Number; i <= currentEndSprint.Number; i++)
            {
                redLineValues.Add(
                    graphDto.SprintChartValues.ElementAt(i-1).Estimated
                    );
                grayLineValues.Add(
                    graphDto.SprintChartValues.ElementAt(i-1).Real
                    );
                XAxes.Add(
                    "Sprint " + i
                    );
            }
        }

/*        public static async ThreadingTask TransferToServer(ObservableCollection<Organization> organizations)
        {

        }*/
    }
}
