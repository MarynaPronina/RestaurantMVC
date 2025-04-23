using System;
using System.Collections.Generic;

namespace RestaurantDomain.Model;

public partial class Table : Entity
{
    public int Id { get; set; }

    public int Number { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
