using System;
using System.Collections.Generic;

namespace NolaProject.Models;

public partial class Payment
{
    public long Id { get; set; }

    public long? SaleId { get; set; }

    public int? PaymentTypeId { get; set; }

    public decimal? Value { get; set; }

    public bool? IsOnline { get; set; }

    public virtual PaymentType? PaymentType { get; set; }

    public virtual Sale? Sale { get; set; }
}
