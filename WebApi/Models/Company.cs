﻿using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Company
{
    public int Id { get; set; }

    public string Customerid { get; set; } = null!;

    public string Companyname { get; set; } = null!;

    public string Taxcode { get; set; } = null!;

    public string Companyaccount { get; set; } = null!;

    public DateTime? Accountissueddate { get; set; }

    public string Cphonenumber { get; set; } = null!;

    public string Caddress { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Loginclient? Loginclient { get; set; }

    public virtual ICollection<Resetpassword> Resetpasswords { get; set; } = new List<Resetpassword>();
}
