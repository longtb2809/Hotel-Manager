using System.Linq;
using System.Windows;
using DataAccess.Models;

namespace FUMiniHotelManagement
{
    public partial class ManagerCustomerWindow : Window
    {
        private readonly FuminiHotelManagementContext _context;
        private readonly Customer _admin;

        public ManagerCustomerWindow(Customer admin)
        {
            InitializeComponent();
            _admin = admin;
            _context = new FuminiHotelManagementContext();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            dgCustomers.ItemsSource = _context.Customers.ToList();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboardWindow dashboard = new AdminDashboardWindow(_admin);
            dashboard.Show();
            this.Close();
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            LoadCustomers();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = new CustomerEditWindow();
            if (window.ShowDialog() == true)
            {
                _context.Customers.Add(window.Customer);
                _context.SaveChanges();
                LoadCustomers();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem is Customer selected)
            {
                var customerInDb = _context.Customers.FirstOrDefault(c => c.CustomerId == selected.CustomerId);
                if (customerInDb == null) return;

                var window = new CustomerEditWindow(customerInDb);
                if (window.ShowDialog() == true)
                {
                    _context.Customers.Update(window.Customer);
                    _context.SaveChanges();
                    LoadCustomers();
                }
            }
            else
            {
                MessageBox.Show("Chọn khách hàng để sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem is Customer selected)
            {
                var confirm = MessageBox.Show($"Xóa khách hàng {selected.FullName}?", "Xác nhận", MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {
                    _context.Customers.Remove(selected);
                    _context.SaveChanges();
                    LoadCustomers();
                }
            }
            else
            {
                MessageBox.Show("Chọn khách hàng để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
