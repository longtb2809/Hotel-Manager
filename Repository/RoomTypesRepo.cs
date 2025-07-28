using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
   
    public class RoomTypesRepo
    {
        FuminiHotelManagementContext _context;
        public RoomTypesRepo(FuminiHotelManagementContext context)
        {
            _context = context;
        }
        public List<RoomType> GetRoomTypes() { 
            return _context.RoomTypes.ToList();
        }

    }
}
