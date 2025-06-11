namespace RestaurantDomain.Model   
{
    public partial class DishOrder : Entity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int DishId { get; set; }
        public int Quantity { get; set; }

        public virtual Dish Dish { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
