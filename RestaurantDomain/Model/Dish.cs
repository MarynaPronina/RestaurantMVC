using System;
using System.Collections.Generic;

namespace RestaurantDomain.Model;

public partial class Dish : Entity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal? Price { get; set; }


    public virtual ICollection<DishCategory> DishCategories { get; set; } = new List<DishCategory>();

    public virtual ICollection<DishOrder> DishOrders { get; set; } = new List<DishOrder>();
}
