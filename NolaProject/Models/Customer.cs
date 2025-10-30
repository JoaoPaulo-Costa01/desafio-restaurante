using System;
using System.Collections.Generic;

namespace NolaProject.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string? CustomerName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Cpf { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Gender { get; set; }

    public bool? AgreeTerms { get; set; }

    public bool? ReceivePromotionsEmail { get; set; }

    public string? RegistrationOrigin { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
