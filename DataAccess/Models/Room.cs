using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public int RoomTypeId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual RoomType RoomType { get; set; } = null!;
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();



}
