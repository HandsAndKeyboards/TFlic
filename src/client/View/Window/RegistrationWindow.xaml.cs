﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TFlic.ViewModel;

namespace TFlic.View
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void BMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            // Application.Current.Shutdown();
        }

        private void RepeatPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((RegistrationWindowViewModel)DataContext).RepeatPassword = RepeatPassword.Password;
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((RegistrationWindowViewModel)DataContext).Password = Password.Password;
        }
    }
}
