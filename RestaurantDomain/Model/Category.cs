using System;
using System.Collections.Generic;

namespace RestaurantDomain.Model;

public partial class Category : Entity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DishCategory> DishCategories { get; set; } = new List<DishCategory>();
}
