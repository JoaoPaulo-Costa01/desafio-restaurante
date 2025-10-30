using System;
using System.Collections.Generic;

namespace NolaProject.Models;

public partial class Brand
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Channel> Channels { get; set; } = new List<Channel>();

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual ICollection<OptionGroup> OptionGroups { get; set; } = new List<OptionGroup>();

    public virtual ICollection<PaymentType> PaymentTypes { get; set; } = new List<PaymentType>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();

    public virtual ICollection<SubBrand> SubBrands { get; set; } = new List<SubBrand>();
}
