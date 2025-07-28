using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FUMiniHotelManagement
{
    public partial class BookingManagerWindow : Window
    {
        private readonly FuminiHotelManagementContext _context;
        private readonly Customer _admin;
        private ObservableCollection<Booking> _bookings;

        public BookingManagerWindow(Customer admin)
        {
            InitializeComponent();
            _admin = admin;
            _context = new FuminiHotelManagementContext();
            LoadBookings();
        }

        private void LoadBookings()
        {
            using (var newContext = new FuminiHotelManagementContext())
            {
                var bookings = newContext.Bookings
                    .Include(b => b.Customer)
                    .Include(b => b.Room)
                    .ToList();

                _bookings = new ObservableCollection<Booking>(bookings);
                dgBookings.ItemsSource = _bookings;
            }
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboardWindow dashboard = new AdminDashboardWindow(_admin);
            dashboard.Show();
            this.Close();
        }
        private void AddBooking_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddBookingWindow();
            addWindow.ShowDialog();

            if (addWindow.IsSaved)
            {
                LoadBookings();
            }
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedBooking = dgBookings.SelectedItem as Booking;
            if (selectedBooking == null)
            {
                MessageBox.Show("Vui lòng chọn booking để sửa.");
                return;
            }

            var booking = _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .FirstOrDefault(b => b.BookingId == selectedBooking.BookingId);

            if (booking == null) return;

            var editWindow = new AddBookingWindow(booking);
            editWindow.ShowDialog();

            if (editWindow.IsSaved)
            {
                LoadBookings(); 
            }
        }

        private void DeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            var selectedBooking = dgBookings.SelectedItem as Booking;
            if (selectedBooking != null)
            {
                var details = _context.BookingDetails.Where(d => d.BookingId == selectedBooking.BookingId);
                _context.BookingDetails.RemoveRange(details);

                _context.Bookings.Remove(selectedBooking);
                _context.SaveChanges();

                _bookings.Remove(selectedBooking);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn booking để xoá.");
            }
        }
    }
}
