using DataAccess.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FUMiniHotelManagement
{
    public partial class AddBookingWindow : Window
    {
        private readonly FuminiHotelManagementContext _context;
        private readonly Booking? _editingBooking;

        public bool IsSaved { get; private set; }

        // Constructor thêm mới
        public AddBookingWindow()
        {
            InitializeComponent();
            _context = new FuminiHotelManagementContext();
            LoadData();
        }

        // Constructor sửa booking
        public AddBookingWindow(Booking bookingToEdit)
        {
            InitializeComponent();
            _context = new FuminiHotelManagementContext();
            _editingBooking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingToEdit.BookingId); // lấy bản ghi mới nhất
            LoadData();

            if (_editingBooking != null)
            {
                cbCustomer.SelectedValue = _editingBooking.CustomerId;
                cbRoom.SelectedValue = _editingBooking.RoomId;
                dpCheckIn.SelectedDate = _editingBooking.CheckInDate;
                dpCheckOut.SelectedDate = _editingBooking.CheckOutDate;
                cbStatus.Text = _editingBooking.Status;
            }
        }


        private void LoadData()
        {
            cbCustomer.ItemsSource = _context.Customers.ToList();
            cbCustomer.DisplayMemberPath = "FullName"; // hoặc cột hiển thị tên
            cbCustomer.SelectedValuePath = "CustomerId";

            cbRoom.ItemsSource = _context.Rooms.ToList();
            cbRoom.DisplayMemberPath = "RoomNumber";
            cbRoom.SelectedValuePath = "RoomId";

            cbStatus.ItemsSource = new string[] { "Pending", "Confirmed", "Cancelled" };
            cbStatus.SelectedIndex = 0;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (cbCustomer.SelectedValue == null || cbRoom.SelectedValue == null || dpCheckIn.SelectedDate == null || dpCheckOut.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            if (_editingBooking == null)
            {
                // Thêm mới
                var newBooking = new Booking
                {
                    CustomerId = (int)cbCustomer.SelectedValue,
                    RoomId = (int)cbRoom.SelectedValue,
                    CheckInDate = dpCheckIn.SelectedDate.Value,
                    CheckOutDate = dpCheckOut.SelectedDate.Value,
                    BookingDate = DateTime.Now,
                    Status = cbStatus.Text
                };

                _context.Bookings.Add(newBooking);
                _context.SaveChanges();

                var detail = new BookingDetail
                {
                    BookingId = newBooking.BookingId,
                    RoomId = newBooking.RoomId
                };
                _context.BookingDetails.Add(detail);
                _context.SaveChanges();
            }
            else
            {
                // Cập nhật
                _editingBooking.CustomerId = (int)cbCustomer.SelectedValue;
                _editingBooking.RoomId = (int)cbRoom.SelectedValue;
                _editingBooking.CheckInDate = dpCheckIn.SelectedDate.Value;
                _editingBooking.CheckOutDate = dpCheckOut.SelectedDate.Value;
                _editingBooking.Status = cbStatus.Text;

                _context.Bookings.Update(_editingBooking);

                var detail = _context.BookingDetails.FirstOrDefault(d => d.BookingId == _editingBooking.BookingId);
                if (detail != null)
                {
                    detail.RoomId = _editingBooking.RoomId;
                    _context.BookingDetails.Update(detail);
                }

                _context.SaveChanges();
            }

            IsSaved = true;
            this.DialogResult = true; // Đặt kết quả dialog là true
            this.Close();
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
