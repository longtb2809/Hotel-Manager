using System;
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
using DataAccess.Models;

namespace FUMiniHotelManagement
{
    /// <summary>
    /// Interaction logic for AdminDashboardWindow.xaml
    /// </summary>
    public partial class AdminDashboardWindow : Window
    {
        Customer _admin;
        public AdminDashboardWindow(Customer admin)
        {
            InitializeComponent();
            _admin = admin;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
        private void ManageCustomers_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new ManagerCustomerWindow(_admin).Show();
            
        }

        private void ManageRooms_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new RoomManagerWindow(_admin).Show();

        }

        private void ManageBookings_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new BookingManagerWindow(_admin).Show();
           
        }

        
    }
}
