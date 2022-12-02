using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TFlic.Command;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.ViewModel
{
    internal class OrganizationWindowViewModel : Base.ViewModelBase
    {
        #region Fields

        int indexOrganization = 0;
        public int IndexOrganization
        {
            get => indexOrganization;
            set => Set(ref indexOrganization, value);
        }

        ObservableCollection<Organization> organizations = new();
        public ObservableCollection<Organization> Organizations
        {
            get => organizations;
            set => Set(ref organizations, value);
        }

        ObservableCollection<Project> currentOrganizationProjects;
        public ObservableCollection<Project> CurrentOrganizationProjects
        {
            get => currentOrganizationProjects;
            set => Set(ref currentOrganizationProjects, value);
        }

        ObservableCollection<Board> currentOrganizationBoards;
        public ObservableCollection<Board> CurrentOrganizationBoards
        {
            get => currentOrganizationBoards;
            set => Set(ref currentOrganizationBoards, value);
        }

        ObservableCollection<Person> currentOrganizationsPeoples;
        public ObservableCollection<Person> CurrentOrganizationsPeoples
        {
            get => currentOrganizationsPeoples;
            set => Set(ref currentOrganizationsPeoples, value);
        }

        #endregion

        #region Commands

        #region Команда добавления организации

        string orgName = string.Empty;
        public string OrgName
        {
            get => orgName;
            set => Set(ref orgName, value);
        }

        string orgDescription = string.Empty;
        public string OrgDescription
        {
            get => orgDescription;
            set => Set(ref orgDescription, value);
        }

        public ICommand AddOrganizationCommand { get; }
        private void OnAddOrganizationCommandExecuted(object p)
        {
            Organizations.Add(
                new Organization()
                {
                    Id = organizations.Count,
                    Name = orgName,
                    Description = orgDescription,
                    projects = new()
                }
            );
        }
        private bool CanAddOrganizationCommandExecute(object p) { return true; }

        #endregion

        #endregion

        #region Constructors

        public OrganizationWindowViewModel()
        {
            AddOrganizationCommand =
                new RelayCommand(OnAddOrganizationCommandExecuted);

            TestData();
        }

        #endregion

        #region Tests

        public void TestData()
        {
            Organizations.Add(
                new()
                {
                    Id = Organizations.Count,
                    Name = "HAK inc.",
                    projects = new()
                }
            );
            //-----------
            Organizations[0].projects.Add(
                new()
                {
                    Id = 0,
                    Name = "Проект1",
                    boards = new()
                }
            );

            Organizations[0].projects.Add(
                new()
                {
                    Id = 1,
                    Name = "Проект2",
                    boards = new()
                }
            );

            Organizations[0].projects.Add(
                new()
                {
                    Id = 2,
                    Name = "Проект3",
                    boards = new()
                }
            );
            //-----------
            Organizations[0].projects[0].boards.Add(
                new()
                {
                    Id = 0,
                    Name = "Доска1",
                    columns = new()
                }
            );
            Organizations[0].projects[0].boards.Add(
                new()
                {
                    Id = 1,
                    Name = "Доска2",
                    columns = new()
                }
            );
            //-----------
            Organizations[0].projects[1].boards.Add(
                new()
                {
                    Id = 0,
                    Name = "Доска1",
                    columns = new()
                }
            );
            Organizations[0].projects[1].boards.Add(
                new()
                {
                    Id = 1,
                    Name = "Доска2",
                    columns = new()
                }
            );
            Organizations[0].projects[1].boards.Add(
                new()
                {
                    Id = 2,
                    Name = "Доска3",
                    columns = new()
                }
            );
            //-----------
            Organizations[0].projects[2].boards.Add(
                new()
                {
                    Id = 0,
                    Name = "Доска1",
                    columns = new()
                }
            );
            Organizations[0].projects[2].boards.Add(
                new()
                {
                    Id = 1,
                    Name = "Доска2",
                    columns = new()
                }
            );
            //-----------
            Organizations[0].peoples.Add(
                new()
                {
                    Id = 0,
                    Name = "Какой-то чувак1",
                }
            );

            Organizations[0].peoples.Add(
                new()
                {
                    Id = 1,
                    Name = "Какой-то чувак2",
                }
            );

            Organizations[0].peoples.Add(
                new()
                {
                    Id = 2,
                    Name = "Какой-то чувак2",
                }
            );
        }

        #endregion
    }
}
