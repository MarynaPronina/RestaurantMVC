using System;
using System.Collections.Generic;

namespace RestaurantDomain.Model;

public partial class Worker : Entity
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? PhoneNumber { get; set; }

    public string? CityName { get; set; }

    public string? StreetName { get; set; }

    public virtual Client Client { get; set; } = null!;
}
