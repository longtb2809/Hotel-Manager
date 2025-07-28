using DataAccess.Models;
using Repository;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FUMiniHotelManagement
{
    public partial class BookingWindow : Window
    {
        private BookingRepo _bookingRepo;
        private RoomRepo _roomRepo;
        private Customer _currentCustomer;

        public BookingWindow(Customer customer, FuminiHotelManagementContext context)
        {
            InitializeComponent();

            _currentCustomer = customer;
            _roomRepo = new RoomRepo(context);
            _bookingRepo = new BookingRepo(context);

            LoadRooms();

            dpCheckIn.SelectedDate = DateTime.Today;
            dpCheckOut.SelectedDate = DateTime.Today.AddDays(1);
            txtTotal.Text = "0";

            dpCheckIn.SelectedDateChanged += (s, e) => UpdatePriceFromRoomType();
            dpCheckOut.SelectedDateChanged += (s, e) => UpdatePriceFromRoomType();
        }

        private void LoadRooms()
        {
            var availableRooms = _roomRepo.GetAvailableRoomsWithRoomType();
            cbRoom.ItemsSource = availableRooms;
            cbRoom.DisplayMemberPath = "RoomNumber";
            cbRoom.SelectedValuePath = "RoomId";

            cbRoom.SelectionChanged += (s, e) =>
            {
                UpdatePriceFromRoomType();
                UpdateRoomDescription();
            };
        }

        private void UpdateRoomDescription()
        {
            if (cbRoom.SelectedItem is Room room && room.RoomType != null)
            {
                txtRoomDescription.Text = room.RoomType.Description ?? "Không có mô tả.";
            }
            else
            {
                txtRoomDescription.Text = "";
            }
        }


        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            if (cbRoom.SelectedItem == null || dpCheckIn.SelectedDate == null || dpCheckOut.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            DateTime checkIn = dpCheckIn.SelectedDate.Value;
            DateTime checkOut = dpCheckOut.SelectedDate.Value;

            DateTime today = DateTime.Today;

            if (checkIn < today)
            {
                MessageBox.Show("Ngày nhận phòng phải từ hôm nay trở đi.");
                return;
            }

            if (checkOut < checkIn)
            {
                MessageBox.Show("Ngày trả phòng phải sau hoặc bằng ngày nhận phòng.");
                return;
            }


            var room = cbRoom.SelectedItem as Room;
            decimal pricePerNight = room.RoomType?.PricePerNight ?? 0;
            int days = (checkOut - checkIn).Days;
            decimal total = pricePerNight * days;

            txtTotal.Text = total.ToString("N0");

            var booking = new Booking
            {
                CustomerId = _currentCustomer.CustomerId,
                RoomId = room.RoomId,
                BookingDate = DateTime.Now,
                CheckInDate = checkIn.Date,
                CheckOutDate = checkOut.Date,
                Status = "Confirmed",
            };

            try
            {
                _bookingRepo.AddBooking(booking);

                var bookingDetail = new BookingDetail
                {
                    BookingId = booking.BookingId,
                    RoomId = room.RoomId,
                    Price = pricePerNight
                };
                _bookingRepo.AddBookingDetail(bookingDetail);

                room.Status = "Booked";
                _roomRepo.UpdateRoom(room);

                MessageBox.Show("Đặt phòng thành công!");
                this.Close();
            }
            catch (Exception ex)
            {
                Exception inner = ex;
                while (inner.InnerException != null)
                {
                    inner = inner.InnerException;
                }
                MessageBox.Show("Lỗi khi đặt phòng:\n" + inner.Message);
            }
        }

        private void UpdatePriceFromRoomType()
        {
            if (cbRoom.SelectedItem is Room room &&
                room.RoomType != null &&
                dpCheckIn.SelectedDate.HasValue &&
                dpCheckOut.SelectedDate.HasValue)
            {
                int nights = (dpCheckOut.SelectedDate.Value - dpCheckIn.SelectedDate.Value).Days;
                if (nights >= 0)
                {
                    decimal total = nights * room.RoomType.PricePerNight;
                    txtTotal.Text = total.ToString("N0");
                }
                else
                {
                    txtTotal.Text = "0";
                }
            }
            else
            {
                txtTotal.Text = "0";
            }
        }
    }
}
