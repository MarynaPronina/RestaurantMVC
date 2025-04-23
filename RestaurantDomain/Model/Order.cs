using System;
using System.Collections.Generic;

namespace RestaurantDomain.Model;

public partial class Order : Entity
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int TableId { get; set; }

    public DateTime? DateTime { get; set; }

    public decimal? Sum { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<DishOrder> DishOrders { get; set; } = new List<DishOrder>();

    public virtual Table Table { get; set; } = null!;
}
