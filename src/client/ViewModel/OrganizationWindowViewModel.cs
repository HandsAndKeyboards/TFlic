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

        ObservableCollection<Project> currentOrganizationsProjects;
        public ObservableCollection<Project> CurrentOrganizationsProjects
        {
            get => currentOrganizationsProjects;
            set => Set(ref currentOrganizationsProjects, value);
        }

        ObservableCollection<Board> currentOrganizationsBoards;
        public ObservableCollection<Board> CurrentOrganizationsBoards
        {
            get => currentOrganizationsBoards;
            set => Set(ref currentOrganizationsBoards, value);
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

        public ICommand AddOrganizationCommand { get; }
        private void OnAddOrganizationCommandExecuted(object p)
        {
            Organizations.Add(
                new Organization()
                {
                    Id = organizations.Count,
                    Name = orgName,
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
        }

        #endregion
    }
}
