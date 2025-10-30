using System;
using System.Collections.Generic;

namespace NolaProject.Models;

public partial class Item
{
    public int Id { get; set; }

    public int? BrandId { get; set; }

    public int? SubBrandId { get; set; }

    public int? CategoryId { get; set; }

    public string? Name { get; set; }

    public string? PosUuid { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<ItemProductSale> ItemProductSales { get; set; } = new List<ItemProductSale>();

    public virtual SubBrand? SubBrand { get; set; }
}
