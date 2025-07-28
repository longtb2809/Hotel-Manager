﻿using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class RoomType
{
    public int RoomTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal PricePerNight { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
   
  
}
