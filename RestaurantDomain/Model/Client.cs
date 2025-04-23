using System;
using System.Collections.Generic;

namespace RestaurantDomain.Model;

public partial class Client : Entity
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? PhoneNumber { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
