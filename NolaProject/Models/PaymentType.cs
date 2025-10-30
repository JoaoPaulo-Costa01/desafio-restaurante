using System;
using System.Collections.Generic;

namespace NolaProject.Models;

public partial class PaymentType
{
    public int Id { get; set; }

    public int? BrandId { get; set; }

    public string? Description { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
