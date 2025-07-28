using System.Windows;
using DataAccess.Models;

namespace FUMiniHotelManagement
{
    public partial class CustomerEditWindow : Window
    {
        public Customer Customer { get; private set; }

        public CustomerEditWindow(Customer customer = null)
        {
            InitializeComponent();
            Customer = customer ?? new Customer();
            LoadData();
        }

        private void LoadData()
        {
            txtFullName.Text = Customer.FullName;
            txtEmail.Text = Customer.Email;
            txtPhone.Text = Customer.Phone;
            txtAddress.Text = Customer.Address;
            txtIdentity.Text = Customer.IdentityNumber;
            txtPassword.Password = Customer.Password;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Customer.FullName = txtFullName.Text;
            Customer.Email = txtEmail.Text;
            Customer.Phone = txtPhone.Text;
            Customer.Address = txtAddress.Text;
            Customer.IdentityNumber = txtIdentity.Text;
            Customer.Password = txtPassword.Password;
            Customer.Role = "Customer";

            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
