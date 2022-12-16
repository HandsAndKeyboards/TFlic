using System.Collections.ObjectModel;
using System.Windows.Input;
using TFlic.Model.Service;
using TFlic.ViewModel.Command;
using TFlic.ViewModel.ViewModelClass;
using TFlic.Model.Transfer;

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

        #region Current collections
        /* Нужны для хранения информации о текущих коллекциях
         * т.е. о колеециях используемых сейчас на интерфейсе
         */ 
        
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

        #region Команда добавления проекта

        string projectName = string.Empty;
        public string ProjectName
        {
            get => projectName;
            set => Set(ref projectName, value);
        }

        string projectDesc = string.Empty;
        public string ProjectDesc
        {
            get => projectDesc;
            set => Set(ref projectDesc, value);
        }

        public ICommand AddProjectCommand { get; }
        private void OnAddProjectCommandExecuted(object p)
        {
            Organizations[indexOrganization].projects.Add(
                new Project()
                {
                    Name = projectName,
                    boards = new()
                });
            ProjectTransferer.TransferToServer(Organizations[indexOrganization].projects, Organizations[indexOrganization].Id);
        }
        private bool CanAddProjectCommandExecute(object p) { return true; }

        #endregion

        #region Команда добавления доски

        string boardName = string.Empty;
        public string BoardName
        {
            get => boardName;
            set => Set(ref boardName, value);
        }

        string boardDesc = string.Empty;
        public string BoardDesc
        {
            get => boardDesc;
            set => Set(ref boardDesc, value);
        }

        private int indexSelectedProject = 0;
        public int IndexSelectedProject
        {
            get => indexSelectedProject;
            set => Set(ref indexSelectedProject, value);
        }

        public ICommand AddBoardCommand { get; }
        private void OnAddBoardCommandExecuted(object p)
        {
            Organizations[indexOrganization].projects[indexSelectedProject].boards.Add(
                new Board()
                {
                    Name = boardName,
                    columns = new()
                });
            BoardTransferer.TransferToServer(Organizations[indexOrganization].projects[indexSelectedProject].boards,
                Organizations[indexOrganization].Id, Organizations[indexOrganization].projects[indexSelectedProject].Id);
        }
        private bool CanAddBoardCommandExecute(object p) { return true; }

        #endregion

        #endregion

        #region Constructors

        public OrganizationWindowViewModel()
        {
            var currentAccountId = AccountService.ReadAccountFromJsonFile().Id;
            OrganizationTransferer.TransferToClient(organizations, (long) currentAccountId);

            AddOrganizationCommand =
                new RelayCommand(OnAddOrganizationCommandExecuted);

            AddProjectCommand =
                new RelayCommand(OnAddProjectCommandExecuted);

            AddBoardCommand =
                new RelayCommand(OnAddBoardCommandExecuted, CanAddBoardCommandExecute);

            // TestData();
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
