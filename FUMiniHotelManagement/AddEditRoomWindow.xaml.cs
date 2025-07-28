using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace FUMiniHotelManagement
{
    public partial class AddEditRoomWindow : Window
    {
        private readonly FuminiHotelManagementContext _context;
        private readonly Room? _room; // có thể null nếu là thêm mới
        private readonly bool _isEdit;

        // Constructor cho THÊM
        public AddEditRoomWindow()
        {
            InitializeComponent();
            _context = new FuminiHotelManagementContext();
            _isEdit = false;
            LoadRoomTypes();
            this.Title = "Thêm Phòng";
        }

        // Constructor cho SỬA
        public AddEditRoomWindow(Room room)
        {
            InitializeComponent();
            _context = new FuminiHotelManagementContext();
            _room = room;
            _isEdit = true;
            LoadRoomTypes();

            if (_room != null)
            {
                this.Title = "Sửa Phòng";
                txtRoomNumber.Text = _room.RoomNumber;
                txtStatus.Text = _room.Status;
                cbRoomType.SelectedValue = _room.RoomTypeId;
            }
        }

        private void LoadRoomTypes()
        {
            var roomTypes = _context.RoomTypes.ToList();
            cbRoomType.ItemsSource = roomTypes;
            cbRoomType.DisplayMemberPath = "TypeName";
            cbRoomType.SelectedValuePath = "RoomTypeId";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoomNumber.Text) ||
                string.IsNullOrWhiteSpace(txtStatus.Text) ||
                cbRoomType.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            if (_isEdit && _room != null)
            {
                // Cập nhật dữ liệu phòng đã có
                _room.RoomNumber = txtRoomNumber.Text;
                _room.Status = txtStatus.Text;
                _room.RoomTypeId = (int)cbRoomType.SelectedValue;
                // EF Core đang theo dõi _room nên không cần gọi Update()
            }
            else
            {
                // Tạo phòng mới
                var newRoom = new Room
                {
                    RoomNumber = txtRoomNumber.Text,
                    Status = txtStatus.Text,
                    RoomTypeId = (int)cbRoomType.SelectedValue
                };
                _context.Rooms.Add(newRoom);
            }

            _context.SaveChanges();
            DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
