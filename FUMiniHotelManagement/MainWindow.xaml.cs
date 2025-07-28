using DataAccess.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Repository.CustomerRepo;

namespace FUMiniHotelManagement
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            
        
            var context = new FuminiHotelManagementContext();
            var loginRepo = new LoginRepository(context);

            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password;

            var user = loginRepo.CheckLogin(email, password);

            if (user != null)
            {
                

                if (user.Role == "Admin")
                {
                    var adminWindow = new AdminDashboardWindow(user);
                    adminWindow.Show();
                }
                else if (user.Role == "Customer")
                {
                    var userWindow = new CustomerDashboardWindow(user);
                    userWindow.Show();
                }
               
                this.Hide();
            }
            else
            {
                MessageBox.Show("Sai email hoặc mật khẩu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}