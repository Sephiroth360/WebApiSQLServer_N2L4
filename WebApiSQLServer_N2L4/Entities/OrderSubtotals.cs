using System;
using System.Collections.Generic;

namespace WebApiSQLServer_N2L4.Entities
{
    public partial class OrderSubtotals
    {
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
