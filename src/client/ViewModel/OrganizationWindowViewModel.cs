﻿#pragma warning disable CS4014

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TFlic.Model.Service;
using TFlic.ViewModel.Command;
using TFlic.ViewModel.ViewModelClass;
using TFlic.Model.Transfer;

using ThreadingTask = System.Threading.Tasks.Task;
using TFlic.Model;
using System.Linq;

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

        string userLogin;
        public string UserLogin
        {
            get => userLogin;
            set => Set(ref userLogin, value);
        }

        string userName;
        public string UserName
        {
            get => userName;
            set => Set(ref userName, value);
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
            OrganizationTransferer.TransferToServer(Organizations);
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

        private async void OnAddProjectCommandExecuted(object p)
        {
            Organizations[indexOrganization].projects.Add(
                new Project()
                {
                    Name = projectName,
                    boards = new()
                });
            try
            {
                await ProjectTransferer.TransferToServer(Organizations[indexOrganization].projects,
                    Organizations[indexOrganization].Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ErrorAddProject");
            }
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

        private int indexSelectedLeftList = 0;
        public int IndexSelectedLeftList
        {
            get => indexSelectedLeftList;
            set => Set(ref indexSelectedLeftList, value);
        }

        public ICommand AddBoardCommand { get; }
        private async void OnAddBoardCommandExecuted(object p)
        {
            Organizations[indexOrganization].projects[indexSelectedLeftList].boards.Add(
                new Board()
                {
                    Name = boardName,
                    columns = new()
                });
            try
            {
                await BoardTransferer.TransferToServer(Organizations[indexOrganization].projects[indexSelectedLeftList].boards,
                    Organizations[indexOrganization].Id, Organizations[indexOrganization].projects[indexSelectedLeftList].Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ErrorAddBoard");
            }
        }
        private bool CanAddBoardCommandExecute(object p) { return true; }

        #endregion

        #region Команда добавления пользователя

        string login = string.Empty;
        public string Login
        {
            get => login;
            set => Set(ref login, value);
        }

        public ICommand AddUserCommand { get; }
        private async void OnUserCommandExecuted(object p)
        {
            Organizations[indexOrganization].peoples.Add(new Person());
            try
            {
                await OrganizationTransferer.TransferToServer(
                    organizations, Organizations[indexOrganization].Id, indexOrganization, login);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, "ErrorAddUser");
            }
        }
        private bool CanUserCommandExecute(object p) { return true; }

        #endregion

        #region Команда изменения сведений об организации 

        public ICommand ChangeOrgInfoCommand { get; }
        private async void OnChangeOrgInfoExecuted(object p)
        {
            try
            {
                Organizations[IndexOrganization].Name = OrgName;
                Organizations[IndexOrganization].Description = OrgDescription;
                await OrganizationTransferer.TransferToServer(
                        Organizations, Organizations[IndexOrganization].Id, IndexOrganization, OrgName,
                        OrgDescription);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ErrorChangeOrgInf");
            }
        }
        private bool CanChangeOrgInfoExecute(object p) 
        {
            return (MessageBoxResult)p == MessageBoxResult.Yes; 
        }

        #endregion

        #region Команда удаления доски

        int indexSelectedBoard;
        public int IndexSelectedBoard
        {
            get => indexSelectedBoard;
            set => Set(ref indexSelectedBoard, value);
        }

        public ICommand DeleteBoardCommand { get; }
        private async void OnDeleteBoardExecuted(object p)
        {
            try
            {
                await BoardTransferer.TransferToServer(
                    Organizations[indexOrganization].Id,
                    Organizations[indexOrganization].projects[indexSelectedLeftList].Id,
                    Organizations[indexOrganization].projects[indexSelectedLeftList].boards[indexSelectedBoard].Id
                    );
                Organizations[indexOrganization].projects[indexSelectedLeftList].boards.RemoveAt(indexSelectedBoard);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ErrorDeleteBoard");
            }
        }
        private bool CanDeleteBoardExecute(object p)
        {
            return (MessageBoxResult)p == MessageBoxResult.Yes;
        }

        #endregion

        #region Команда удаления проекта

        public ICommand DeleteProjectCommand { get; }
        private async void OnDeleteProjectExecuted(object p)
        {
            try
            {
                await ProjectTransferer.TransferToServer(
                    Organizations[indexOrganization].Id,
                    Organizations[indexOrganization].projects[indexSelectedLeftList].Id
                    );
                Organizations[indexOrganization].projects.RemoveAt(indexSelectedLeftList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ErrorDeleteProject");
            }
        }
        private bool CanDeleteProjectExecute(object p)
        {
            return (MessageBoxResult)p == MessageBoxResult.Yes;
        }

        #endregion

        #region Команда удаления проекта

        public ICommand DeletePersonCommand { get; }
        private async void OnDeletePersonExecuted(object p)
        {
            try
            {
                await OrganizationTransferer.TransferToServer(Organizations[indexOrganization].Id, 
                    Organizations[indexOrganization].peoples[indexSelectedLeftList].Id);
                Organizations[indexOrganization].peoples.RemoveAt(indexSelectedLeftList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ErrorDeletePerson");
            }
        }
        private bool CanDeletePersonExecute(object p)
        {
            return (MessageBoxResult)p == MessageBoxResult.Yes;
        }

        #endregion

        #region Команда изменения роли пользователя

        int selectedIndexUserGroupsCB = 0;
        public int SelectedIndexUserGroupsCB
        {
            get => selectedIndexUserGroupsCB;
            set => Set(ref selectedIndexUserGroupsCB, value);
        }

        Person selectedUser;
        public Person SelectedUser
        {
            get => selectedUser;
            set => Set(ref selectedUser, value);
        }

        UserGroupDto selectedUserGroup;
        public UserGroupDto SeletedUserGroup
        {
            get => selectedUserGroup;
            set => Set(ref selectedUserGroup, value);
        }

        #region Добавление роли
        public ICommand AddUserInUserGroup { get; }
        public void OnAddUserInUserGroupExecuted(object p)
        {
            try
            {
                Organizations[indexOrganization]
                    .userGroups[SelectedIndexUserGroupsCB]
                    .Accounts.Add(selectedUser.Id);

                OrganizationTransferer.AddUserInUserGroupTransferToServer(Organizations[indexOrganization].Id, SeletedUserGroup.LocalId, selectedUser.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ErrorAdd");
            }
        }
        public bool CanAddUserInUserGroupExecute(object p)
        {
            bool result = true;
            
            for (int i = 0; i < selectedUserGroup.Accounts.Count && result; i++)
            {
                if (selectedUserGroup.Accounts.ElementAt(i) == selectedUser.Id)
                    result = false;
            }

            return result;
        }
        #endregion

        #region Удаление роли 
        public ICommand RemoveUserInUserGroup { get; }
        public void OnRemoveUserInUserGroupExecuted(object p)
        {
            try
            {
                Organizations[indexOrganization]
                    .userGroups[SelectedIndexUserGroupsCB]
                    .Accounts.Remove(selectedUser.Id);

                OrganizationTransferer.RemoveUserInUserGroupTransferToServer(Organizations[indexOrganization].Id, SeletedUserGroup.LocalId, selectedUser.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ErrorRemove");
            }
        }
        public bool CanRemoveUserInUserGroupExecute(object p)
        {
            bool result = false;

            for (int i = 0; i < selectedUserGroup.Accounts.Count && !result; i++)
            {
                if (selectedUserGroup.Accounts.ElementAt(i) == selectedUser.Id)
                    result = true;
            }

            return result;
        }
        #endregion

        #endregion

        #region Команда выхода из аккаунта

        public ICommand LogoutCommand { get; }
        private void OnLogoutExecuted(object p)
        {
            try
            {
                AccountService.DeleteTokens();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ErrorLogout");
            }
        }
        private bool CanLogoutExecute(object p)
        {
            return (MessageBoxResult)p == MessageBoxResult.Yes;
        }

        #endregion

        #endregion

        #region Constructors

        public OrganizationWindowViewModel()
        {
            AddOrganizationCommand =
                new RelayCommand(OnAddOrganizationCommandExecuted);

            AddProjectCommand =
                new RelayCommand(OnAddProjectCommandExecuted);

            AddBoardCommand =
                new RelayCommand(OnAddBoardCommandExecuted, CanAddBoardCommandExecute);

            AddUserCommand =
                new RelayCommand(OnUserCommandExecuted, CanUserCommandExecute);

            ChangeOrgInfoCommand =
                new RelayCommand(OnChangeOrgInfoExecuted, CanChangeOrgInfoExecute);

            DeleteBoardCommand =
                new RelayCommand(OnDeleteBoardExecuted, CanDeleteBoardExecute);

            DeleteProjectCommand =
                new RelayCommand(OnDeleteProjectExecuted, CanDeleteProjectExecute);

            DeletePersonCommand =
                new RelayCommand(OnDeletePersonExecuted, CanDeletePersonExecute);

            LogoutCommand =
                new RelayCommand(OnLogoutExecuted, CanLogoutExecute);

            AddUserInUserGroup =
                new RelayCommand(OnAddUserInUserGroupExecuted, CanAddUserInUserGroupExecute);

            RemoveUserInUserGroup =
                new RelayCommand(OnRemoveUserInUserGroupExecuted, CanRemoveUserInUserGroupExecute);

            LoadData();
            // TestData();
        }
        
        #endregion

        #region Methods
        private async ThreadingTask LoadData()
        {
            var currentAccountId = AccountService.ReadAccountFromJsonFile().Id;
            try { await OrganizationTransferer.TransferToClient(organizations, (long)currentAccountId); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ErrorLoadData"); }

            UserLogin = AccountService.ReadAccountFromJsonFile().Login;
            UserName = AccountService.ReadAccountFromJsonFile().Name;
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
