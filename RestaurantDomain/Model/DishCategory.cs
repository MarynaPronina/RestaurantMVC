using System;
using System.Collections.Generic;

namespace RestaurantDomain.Model;

public partial class DishCategory : Entity
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public int DishId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Dish Dish { get; set; } = null!;
}
