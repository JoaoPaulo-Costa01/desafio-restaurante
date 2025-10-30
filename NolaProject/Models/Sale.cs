using System;
using System.Collections.Generic;

namespace NolaProject.Models;

public partial class Sale
{
    public long Id { get; set; }

    public int? StoreId { get; set; }

    public int? CustomerId { get; set; }

    public int? ChannelId { get; set; }

    public string? CustomerName { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? SaleStatusDesc { get; set; }

    public decimal? TotalAmountItems { get; set; }

    public decimal? TotalDiscount { get; set; }

    public decimal? TotalIncrease { get; set; }

    public decimal? DeliveryFee { get; set; }

    public decimal? ServiceTaxFee { get; set; }

    public decimal? TotalAmount { get; set; }

    public decimal? ValuePaid { get; set; }

    public int? ProductionSeconds { get; set; }

    public int? DeliverySeconds { get; set; }

    public string? DiscountReason { get; set; }

    public int? PeopleQuantity { get; set; }

    public string? Origin { get; set; }

    public virtual Channel? Channel { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<DeliveryAddress> DeliveryAddresses { get; set; } = new List<DeliveryAddress>();

    public virtual ICollection<DeliverySale> DeliverySales { get; set; } = new List<DeliverySale>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<ProductSale> ProductSales { get; set; } = new List<ProductSale>();

    public virtual Store? Store { get; set; }
}
