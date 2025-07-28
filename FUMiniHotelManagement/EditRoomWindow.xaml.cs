using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace FUMiniHotelManagement
{
    public partial class EditRoomWindow : Window
    {
        private readonly FuminiHotelManagementContext _context;
        private readonly Room _room;

        public EditRoomWindow(Room room)
        {
            InitializeComponent();
            _context = new FuminiHotelManagementContext();
            _room = _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefault(r => r.RoomId == room.RoomId);

            LoadRoomTypes();
            BindRoomData();
        }

        private void LoadRoomTypes()
        {
            var roomTypes = _context.RoomTypes.ToList();
            cbRoomType.ItemsSource = roomTypes;
        }

        private void BindRoomData()
        {
            if (_room != null)
            {
                txtRoomNumber.Text = _room.RoomNumber;
                txtStatus.Text = _room.Status;
                cbRoomType.SelectedValue = _room.RoomTypeId;
                txtDescription.Text = _room.RoomType.Description;
                txtPrice.Text = _room.RoomType.PricePerNight.ToString();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_room == null) return;

            if (string.IsNullOrWhiteSpace(txtRoomNumber.Text) ||
                string.IsNullOrWhiteSpace(txtStatus.Text) ||
                cbRoomType.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                !decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ và đúng định dạng.");
                return;
            }

            _room.RoomNumber = txtRoomNumber.Text;
            _room.Status = txtStatus.Text;
            _room.RoomTypeId = (int)cbRoomType.SelectedValue;

            var selectedRoomType = _context.RoomTypes.FirstOrDefault(rt => rt.RoomTypeId == _room.RoomTypeId);
            if (selectedRoomType != null)
            {
                selectedRoomType.Description = txtDescription.Text;
                selectedRoomType.PricePerNight = price;
                _context.RoomTypes.Update(selectedRoomType);
            }

            _context.Rooms.Update(_room);
            _context.SaveChanges();

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
