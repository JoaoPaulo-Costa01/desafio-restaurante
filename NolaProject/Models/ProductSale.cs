using System;
using System.Collections.Generic;

namespace NolaProject.Models;

public partial class ProductSale
{
    public long Id { get; set; }

    public long? SaleId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? BasePrice { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? Observations { get; set; }

    public virtual ICollection<ItemProductSale> ItemProductSales { get; set; } = new List<ItemProductSale>();

    public virtual Product? Product { get; set; }

    public virtual Sale? Sale { get; set; }
}
