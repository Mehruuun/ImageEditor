using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Interfaces;
using WpfApp1.Services;

namespace WpfApp1.ViewModels
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        public ICommand CreateAccCommand { get; set; }
        public event EventHandler? AccountCreated;
        private void CreateAcc(object parameter)
        {
            
            if (ConfirmPassword != Password )
            {
                MessageBox.Show("رمز عبور مطابقت ندارد");
                return;
            }
            var db = new DatabaseService();
            db.InsertUser(UserName, Password);
           
        AccountCreated?.Invoke(this, EventArgs.Empty);
            
            
            
        }
        public SignUpViewModel()
        {
            CreateAccCommand = new RelayCommand(CreateAcc);
        }
        
        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
            
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
            
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
        }
        
    }
}