using System;
using System.Collections.Generic;

namespace NolaProject.Models;

public partial class DeliveryAddress
{
    public long Id { get; set; }

    public long? SaleId { get; set; }

    public long? DeliverySaleId { get; set; }

    public string? Street { get; set; }

    public string? Number { get; set; }

    public string? Complement { get; set; }

    public string? Neighborhood { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public virtual DeliverySale? DeliverySale { get; set; }

    public virtual Sale? Sale { get; set; }
}
