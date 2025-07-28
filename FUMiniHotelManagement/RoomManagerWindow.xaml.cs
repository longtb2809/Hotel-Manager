using System.Linq;
using System.Windows;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace FUMiniHotelManagement
{
    public partial class RoomManagerWindow : Window
    {
        private readonly FuminiHotelManagementContext _context;
        private readonly Customer _admin;

        public RoomManagerWindow(Customer admin)
        {
            InitializeComponent();
            _admin = admin;
            _context = new FuminiHotelManagementContext();
            LoadRooms();
        }

        private void LoadRooms()
        {
            var roomList = _context.Rooms
                .Include(r => r.RoomType)
                .ToList();

            dgRooms.ItemsSource = roomList;
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            LoadRooms();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboardWindow dashboard = new AdminDashboardWindow(_admin);
            dashboard.Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditRoomWindow(null); // truyền null để tạo mới
            if (addWindow.ShowDialog() == true)
            {
                LoadRooms();
            }

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoom = dgRooms.SelectedItem as Room;
            if (selectedRoom == null)
            {
                MessageBox.Show("Vui lòng chọn phòng cần sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var editWindow = new EditRoomWindow(selectedRoom);
            if (editWindow.ShowDialog() == true)
            {
                LoadRooms();
            }

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoom = dgRooms.SelectedItem as Room;
            if (selectedRoom == null)
            {
                MessageBox.Show("Vui lòng chọn phòng cần xoá.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc muốn xoá phòng này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Rooms.Remove(selectedRoom);
                    _context.SaveChanges();
                    MessageBox.Show("Xoá thành công.", "Thông báo");
                    LoadRooms();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xoá: " + ex.Message);
                }
            }

        }
    }
}
