using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Interfaces;
using WpfApp1.Services;

namespace WpfApp1.ViewModels
{
    internal class SignInViewModel : INotifyPropertyChanged
    {
        
        private string _userName { get; set; }
        private string _password { get; set; }
        public event EventHandler? SuccLog;


        public string UserName
        {  get => _userName;
                set
                { 
                _userName = value;
               
                }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
            }
           
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Login (object parameter)
        {
            var db = new DatabaseService();
            bool isValid = db.ValidateUser(UserName, Password);
            if (isValid)
            {
                
                MessageBox.Show("ورود موفق");
                SuccLog?.Invoke(this, EventArgs.Empty);
                

            }
            else
            {
                MessageBox.Show("نام کاربری یا رمز عبور اشتباه است");
            }
            
            //db.ValidateUser(UserName, Password);
        }
        
        public  ICommand LoginCommand { get; set; }
        public SignInViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

    }
}
