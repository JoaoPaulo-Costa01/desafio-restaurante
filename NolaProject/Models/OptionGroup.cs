using System;
using System.Collections.Generic;

namespace NolaProject.Models;

public partial class OptionGroup
{
    public int Id { get; set; }

    public int? BrandId { get; set; }

    public string? Name { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual ICollection<ItemProductSale> ItemProductSales { get; set; } = new List<ItemProductSale>();
}
