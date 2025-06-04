using System;
using System.Collections.Generic;

namespace PhoneStore.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public int? ShippingAddressId { get; set; }

    public int? CouponId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual Coupon? Coupon { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ShippingAddress? ShippingAddress { get; set; }
}
