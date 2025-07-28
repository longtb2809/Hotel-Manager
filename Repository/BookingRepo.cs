using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{

    public class BookingRepo
    {
        FuminiHotelManagementContext _context;
        public BookingRepo(FuminiHotelManagementContext context)
        {
            _context = context;
        }
        public List<Booking> GetAllBookings()
        {
            return _context.Bookings.ToList();
        }
        public void AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }
        public void AddBookingDetail(BookingDetail detail)
        {
            _context.BookingDetails.Add(detail);
            _context.SaveChanges();
        }

    }



}
