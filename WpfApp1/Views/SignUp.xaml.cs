using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using WpfApp1.ViewModels;

namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public SignUp()
        {
            InitializeComponent();
            var vm = new SignUpViewModel();
            DataContext = vm;
            
            vm.AccountCreated += Vm_Accountcreated;

        }
        private void Vm_Accountcreated (object sender, EventArgs e)
            {
            SignIn sign = new SignIn();
            sign.Show();
            this.Close();

            }


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }

        private void ConfirmPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel vm)
            {
                vm.ConfirmPassword = ((PasswordBox)sender).Password;
            }
        }

      
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"UserName: {((SignUpViewModel)DataContext).UserName}");
        }
    }
}
