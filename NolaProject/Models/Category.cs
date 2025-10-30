using System;
using System.Collections.Generic;

namespace NolaProject.Models;

public partial class Category
{
    public int Id { get; set; }

    public int? BrandId { get; set; }

    public string? Name { get; set; }

    public char? Type { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
