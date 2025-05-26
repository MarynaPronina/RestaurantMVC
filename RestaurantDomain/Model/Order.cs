using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RestaurantDomain.Model;

public partial class Order : Entity
{
    public int Id { get; set; }

    [Required]
    public int ClientId { get; set; }

    [Required]
    public int TableId { get; set; }

    public DateTime? DateTime { get; set; }

    public decimal? Sum { get; set; }

    [ValidateNever]
    public virtual Client Client { get; set; } = null!;

    [ValidateNever]
    public virtual Table Table { get; set; } = null!;

    [ValidateNever]
    public virtual ICollection<DishOrder> DishOrders { get; set; } = new List<DishOrder>();
}
