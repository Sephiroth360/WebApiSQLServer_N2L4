﻿using System;
using System.Collections.Generic;

namespace WebApiSQLServer_N2L4.Entities
{
    public partial class ProductsAboveAveragePrice
    {
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
