using System.Windows;
using DataAccess.Models;

namespace FUMiniHotelManagement
{
    /// <summary>
    /// Interaction logic for CustomerDashboardWindow.xaml
    /// </summary>
    public partial class CustomerDashboardWindow : Window
    {
        private Customer currentcustomer;
        private FuminiHotelManagementContext context; // ✅ Khai báo

        public CustomerDashboardWindow(Customer customer)
        {
            InitializeComponent();
            currentcustomer = customer;

            // ✅ Khởi tạo context
            context = new FuminiHotelManagementContext();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var viewProfileWindow = new ViewProfileWindow(currentcustomer);
            viewProfileWindow.Show();
        }

        private void btnBooking_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var bookingWindow = new BookingWindow(currentcustomer, context);
            bookingWindow.ShowDialog();
            this.Show();
        }

        private void btnBookingHistory_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var bookingHistoryWindow = new BookingHistoryWindow(currentcustomer, context); // ✅ truyền context đúng
            bookingHistoryWindow.ShowDialog();
            this.Show();
        }
    }
}
