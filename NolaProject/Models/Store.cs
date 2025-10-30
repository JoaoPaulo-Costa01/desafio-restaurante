using System;
using System.Collections.Generic;

namespace NolaProject.Models;

public partial class Store
{
    public int Id { get; set; }

    public int? BrandId { get; set; }

    public int? SubBrandId { get; set; }

    public string? Name { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? District { get; set; }

    public string? AddressStreet { get; set; }

    public string? AddressNumber { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsOwn { get; set; }

    public DateOnly? CreationDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual SubBrand? SubBrand { get; set; }
}
