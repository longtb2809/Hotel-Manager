using DataAccess.Models;
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

namespace FUMiniHotelManagement
{
    /// <summary>
    /// Interaction logic for ViewProfile.xaml
    /// </summary>
    public partial class ViewProfileWindow : Window
    {
        Customer currentcustomer;

        public ViewProfileWindow(Customer customer)
        {
            InitializeComponent();
            currentcustomer = customer;
            LoadData();
        }
        public void LoadData()
        {
            txtAddress.Text = currentcustomer.Address;
            txtEmail.Text = currentcustomer.Email;
            txtFullName.Text = currentcustomer.FullName;
            txtPhone.Text = currentcustomer.Phone;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var customerDashboardWindow = new CustomerDashboardWindow(currentcustomer);
            customerDashboardWindow.Show();

        }
    }
}
