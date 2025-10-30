using System;
using System.Collections.Generic;

namespace NolaProject.Models;

public partial class Channel
{
    public int Id { get; set; }

    public int? BrandId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public char? Type { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
