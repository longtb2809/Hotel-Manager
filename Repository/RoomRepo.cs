using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class RoomRepo
    {
        private readonly FuminiHotelManagementContext _context;

        public RoomRepo(FuminiHotelManagementContext context)
        {
            _context = context;
        }

       
        public Room? GetRoomById(int roomId)
        {
            return _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefault(r => r.RoomId == roomId);
        }

      
        public void UpdateRoom(Room room)
        {
            _context.Rooms.Update(room);
            _context.SaveChanges();
        }

    
        public List<Room> GetAllRoomsIncludingRoomType()
        {
            return _context.Rooms
                .Include(r => r.RoomType)
                .ToList();
        }


        public List<Room> GetAvailableRoomsWithRoomType()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            return _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.Bookings)
                .Where(r => r.Status == "Available" &&
                            !r.Bookings.Any(b =>
                                b.Status != "Cancelled" &&
                                DateOnly.FromDateTime(b.CheckInDate) <= today &&
                                DateOnly.FromDateTime(b.CheckOutDate) > today))
                .ToList();
        }

    }
}
