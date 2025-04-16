﻿using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Requirement
{
    public int Id { get; set; }

    public string Requirementsid { get; set; } = null!;

    public string Requirementsstatus { get; set; } = null!;

    public DateTime? Dateofrequest { get; set; }

    public string Descriptionofrequest { get; set; } = null!;

    public string Customerid { get; set; } = null!;

    public string SupportCode { get; set; } = null!;

    public virtual ICollection<Assign> Assigns { get; set; } = new List<Assign>();

    public virtual Company Customer { get; set; } = null!;

    public virtual ICollection<Historyreq> Historyreqs { get; set; } = new List<Historyreq>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual SupportType SupportCodeNavigation { get; set; } = null!;
}
