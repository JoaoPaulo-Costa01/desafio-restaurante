using System;
using System.Collections.Generic;

namespace NolaProject.Models;

public partial class DeliverySale
{
    public long Id { get; set; }

    public long? SaleId { get; set; }

    public string? CourierName { get; set; }

    public string? CourierPhone { get; set; }

    public string? CourierType { get; set; }

    public string? DeliveryType { get; set; }

    public string? Status { get; set; }

    public decimal? DeliveryFee { get; set; }

    public decimal? CourierFee { get; set; }

    public virtual ICollection<DeliveryAddress> DeliveryAddresses { get; set; } = new List<DeliveryAddress>();

    public virtual Sale? Sale { get; set; }
}
