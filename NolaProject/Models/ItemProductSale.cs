using System;
using System.Collections.Generic;

namespace NolaProject.Models;

public partial class ItemProductSale
{
    public long Id { get; set; }

    public long? ProductSaleId { get; set; }

    public int? ItemId { get; set; }

    public int? OptionGroupId { get; set; }

    public int? Quantity { get; set; }

    public decimal? AdditionalPrice { get; set; }

    public decimal? Price { get; set; }

    public int? Amount { get; set; }

    public virtual Item? Item { get; set; }

    public virtual OptionGroup? OptionGroup { get; set; }

    public virtual ProductSale? ProductSale { get; set; }
}
