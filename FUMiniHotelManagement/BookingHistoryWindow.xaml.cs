using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FUMiniHotelManagement
{
    public partial class BookingHistoryWindow : Window
    {
        private readonly FuminiHotelManagementContext _context;
        private readonly Customer _customer;
        private List<BookingDisplayDto> _bookingData;

        public BookingHistoryWindow(Customer customer, FuminiHotelManagementContext context)
        {
            InitializeComponent();
            _context = context;
            _customer = customer;

            LoadBookingHistory();

            // Placeholder cho tìm kiếm
            txtSearch.TextChanged += (s, e) =>
            {
                txtSearchPlaceholder.Visibility = string.IsNullOrEmpty(txtSearch.Text)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            };
        }

        private void LoadBookingHistory()
        {
            var bookings = _context.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r.RoomType)
                .Include(b => b.BookingDetails)
                .Where(b => b.CustomerId == _customer.CustomerId && b.Status != "Cancelled")
                .OrderByDescending(b => b.BookingDate)
                .ToList();

            _bookingData = bookings
                .Select(b => new BookingDisplayDto
                {
                    BookingId = b.BookingId,
                    RoomNumber = b.Room?.RoomNumber ?? "",
                    RoomType = b.Room?.RoomType?.TypeName ?? "",
                    BookingDate = b.BookingDate.ToString("dd/MM/yyyy"),
                    CheckInDate = b.CheckInDate.ToString("dd/MM/yyyy"),
                    CheckOutDate = b.CheckOutDate.ToString("dd/MM/yyyy"),
                    PricePerNight = b.BookingDetails.FirstOrDefault()?.Price
                                    ?? b.Room?.RoomType?.PricePerNight
                                    ?? 0,
                    Status = b.Status
                })
                .ToList();

            dgBookingHistory.ItemsSource = _bookingData;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            dgBookingHistory.ItemsSource = string.IsNullOrEmpty(keyword)
                ? _bookingData
                : _bookingData.Where(b =>
                       b.RoomType.ToLower().Contains(keyword) ||
                       b.RoomNumber.ToLower().Contains(keyword)
                   ).ToList();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancelBooking_Click(object sender, RoutedEventArgs e)
        {
            if (dgBookingHistory.SelectedItem is BookingDisplayDto selectedBooking)
            {
                var confirm = MessageBox.Show(
                    $"Bạn có chắc muốn huỷ đặt phòng {selectedBooking.RoomNumber}?",
                    "Xác nhận huỷ",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                {
                    var booking = _context.Bookings
                        .Include(b => b.Room)
                        .FirstOrDefault(b => b.BookingId == selectedBooking.BookingId);

                    if (booking != null && booking.Status != "Cancelled")
                    {
                        booking.Status = "Cancelled";

                        // Cập nhật lại trạng thái phòng
                        if (booking.Room != null)
                        {
                            booking.Room.Status = "Available";
                        }

                        _context.SaveChanges();
                        MessageBox.Show("Huỷ đặt phòng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadBookingHistory();
                    }
                    else
                    {
                        MessageBox.Show("Không thể huỷ đặt phòng này!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một đặt phòng để huỷ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    public class BookingDisplayDto
    {
        public int BookingId { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public string BookingDate { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public decimal PricePerNight { get; set; }
        public string Status { get; set; }
    }
}
