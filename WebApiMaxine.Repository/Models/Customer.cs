using System;
using System.Collections.Generic;

namespace WebApiMaxine.Repository.Models;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string? ContactName { get; set; }

    public string Address { get; set; } = null!;

    public string? City { get; set; }

    public string? Region { get; set; }

    public int? PostalCode { get; set; }

    public string? Country { get; set; }

    public string Phone { get; set; } = null!;

    public string? Fax { get; set; }
}
