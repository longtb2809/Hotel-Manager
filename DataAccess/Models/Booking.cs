using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public DateTime BookingDate { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public int RoomId { get; set; }
    public virtual Customer Customer { get; set; } = null!;
    public virtual Room Room { get; set; }

}
