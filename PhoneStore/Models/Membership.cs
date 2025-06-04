using System;
using System.Collections.Generic;

namespace PhoneStore.Models;

public partial class Membership
{
    public int MembershipId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
